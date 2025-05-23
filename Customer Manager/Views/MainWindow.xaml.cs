using Customer_Manager.Data;
using Customer_Manager.Helpers;
using Customer_Manager.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Customer_Manager.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly string _editor;
    private ObservableCollection<Customer> _customers = new();
    private Customer? _customerBeingEdited = null;

    public MainWindow(string editor)
    {
        this.InitializeComponent();
        _editor = editor;

        CustomerDataGrid.DoubleTapped += CustomerDataGrid_DoubleTapped;


        // Hook up size change for diagnostics
        this.SizeChanged += MainWindow_SizeChanged;

        UserLabel.Text = $"Logged in as: {_editor}";
        LoadCustomers();
    }

    private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
    {
        var width = e.Size.Width;
        StateLabel.Text = $"Layout: {(width < 600 ? "Narrow" : "Wide")}";
    }

    private async void AddCustomer_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CustomerDialog
        {
            XamlRoot = this.Content.XamlRoot // required for WinUI 3 dialogs
        };

        var result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Month = PathHelper.GetMonth(),
                Date = PathHelper.GetDay(),
                Editor = _editor,
                Name = dialog.CustomerName,
                Email = dialog.CustomerEmail,
                SME = dialog.IsSme ? "IG" : "",
                SV = dialog.IsSv ? "HC" : ""
            };

            string dbPath = PathHelper.GetUserDbPath(_editor);
            var repo = new CustomerRepository(dbPath);
            repo.AddCustomer(customer);
            PathHelper.GetCustomerFolder(_editor, customer.Name);

            LoadCustomers();
        }
    }


    private void EmailTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            AddCustomer_Click(sender, new RoutedEventArgs());
        }
    }   

    private async void CustomerDataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (CustomerDataGrid.SelectedItem is Customer selected)
        {
            var dialog = new CustomerDialog
            {
                XamlRoot = this.Content.XamlRoot
            };

            dialog.SetInitialData(selected.Name, selected.Email, selected.SME, selected.SV);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string oldName = selected.Name;

                selected.Name = dialog.CustomerName;
                selected.Email = dialog.CustomerEmail;
                selected.SME = dialog.IsSme ? "IG" : "";
                selected.SV = dialog.IsSv ? "HC" : "";

                string dbPath = PathHelper.GetUserDbPath(_editor);
                var repo = new CustomerRepository(dbPath);
                repo.UpdateCustomer(selected);

                if (oldName != selected.Name)
                {
                    string oldPath = PathHelper.GetCustomerFolderPathOnly(_editor, oldName);
                    string newPath = PathHelper.GetCustomerFolderPathOnly(_editor, selected.Name);

                    if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
                    {
                        Directory.Move(oldPath, newPath);
                    }
                }

                LoadCustomers();
            }
        }
    }


    private void LoadCustomers()
    {
        string dbPath = PathHelper.GetUserDbPath(_editor);
        var repo = new CustomerRepository(dbPath);
        var customerList = repo.GetCustomers();

        _customers = new ObservableCollection<Customer>(customerList);
        CustomerDataGrid.ItemsSource = _customers;
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadCustomers();
    }

    private async void DeleteRow_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is Customer selectedCustomer)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = $"Are you sure you want to delete {selectedCustomer.Name}?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var repo = new CustomerRepository(PathHelper.GetUserDbPath(_editor));
                repo.DeleteCustomer(selectedCustomer.Id);

                // Delete customer folder
                string folderPath = PathHelper.GetCustomerFolderPathOnly(_editor, selectedCustomer.Name);
                if (Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch (Exception ex)
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Folder Delete Error",
                            Content = $"Could not delete folder: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }

                LoadCustomers(); // Refresh the table
            }
        }
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        string query = SearchTextBox.Text.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(query))
        {
            CustomerDataGrid.ItemsSource = _customers;
            return;
        }

        var filtered = _customers
            .Where(c =>
                (!string.IsNullOrWhiteSpace(c.Name) && c.Name.ToLower().Contains(query)) ||
                (!string.IsNullOrWhiteSpace(c.Email) && c.Email.ToLower().Contains(query)))
            .ToList();

        CustomerDataGrid.ItemsSource = filtered;
    }
}