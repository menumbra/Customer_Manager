using Customer_Manager.Data;
using Customer_Manager.Helpers;
using Customer_Manager.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Customer_Manager.Views;

public sealed partial class MainWindow : Window
{
    private readonly string _editor;
    private ObservableCollection<Customer> _customers = new();

    public MainWindow(string editor)
    {
        this.InitializeComponent();
        _editor = editor;
        LoadCustomers();
    }

    private void LoadCustomers()
    {
        string dbPath = PathHelper.GetUserDbPath(_editor);
        var repo = new CustomerRepository(dbPath);
        var list = repo.GetCustomers();
        _customers = new ObservableCollection<Customer>(list);
        CustomerDataGrid.ItemsSource = _customers;
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        string query = SearchTextBox.Text.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(query))
        {
            CustomerDataGrid.ItemsSource = _customers;
            return;
        }

        var filtered = _customers.Where(c =>
            (!string.IsNullOrWhiteSpace(c.Name) && c.Name.ToLower().Contains(query)) ||
            (!string.IsNullOrWhiteSpace(c.Email) && c.Email.ToLower().Contains(query))).ToList();

        CustomerDataGrid.ItemsSource = filtered;
    }

    private async void AddCustomer_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CustomerDialog
        {
            XamlRoot = this.Content.XamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = dialog.CustomerName,
                Email = dialog.CustomerEmail,
                SME = dialog.IsSme ? "IG" : "",
                SV = dialog.IsSv ? "HC" : "",
                Month = PathHelper.GetMonth(),
                Date = PathHelper.GetDay(),
                Editor = _editor
            };

            string dbPath = PathHelper.GetUserDbPath(_editor);
            var repo = new CustomerRepository(dbPath);
            repo.AddCustomer(customer);

            PathHelper.GetCustomerFolder(_editor, customer.Name);
            LoadCustomers();
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadCustomers();
    }


    private async void DeleteRow_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is Customer customer)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = $"Delete {customer.Name}?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var repo = new CustomerRepository(PathHelper.GetUserDbPath(_editor));
                repo.DeleteCustomer(customer.Id);

                string folderPath = PathHelper.GetCustomerFolderPathOnly(_editor, customer.Name);
                if (Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch (Exception ex)
                    {
                        await new ContentDialog
                        {
                            Title = "Folder Delete Error",
                            Content = $"Could not delete folder: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        }.ShowAsync();
                    }
                }

                LoadCustomers();
            }
        }
    }

    private async void CustomerDataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (CustomerDataGrid.SelectedItem is Customer customer)
        {
            var dialog = new CustomerDialog
            {
                XamlRoot = this.Content.XamlRoot
            };

            dialog.SetInitialData(customer.Name, customer.Email, customer.SME, customer.SV);

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string oldName = customer.Name;

                customer.Name = dialog.CustomerName;
                customer.Email = dialog.CustomerEmail;
                customer.SME = dialog.IsSme ? "IG" : "";
                customer.SV = dialog.IsSv ? "HC" : "";

                var repo = new CustomerRepository(PathHelper.GetUserDbPath(_editor));
                repo.UpdateCustomer(customer);

                if (oldName != customer.Name)
                {
                    string oldPath = PathHelper.GetCustomerFolderPathOnly(_editor, oldName);
                    string newPath = PathHelper.GetCustomerFolderPathOnly(_editor, customer.Name);

                    if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
                    {
                        Directory.Move(oldPath, newPath);
                    }
                }

                LoadCustomers();
            }
        }
    }

}
