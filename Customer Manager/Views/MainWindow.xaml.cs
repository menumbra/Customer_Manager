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

        LoadCustomers();
    }

    private async void AddCustomer_Click(object sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text.Trim();
        string email = EmailTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = "Name and Email are required.",
                CloseButtonText = "OK",
                XamlRoot = NameTextBox.XamlRoot
            };
            await dialog.ShowAsync();
            return;
        }

        string dbPath = PathHelper.GetUserDbPath(_editor);
        var repo = new CustomerRepository(dbPath);

        if (_customerBeingEdited is not null)
        {
            // We're editing an existing customer
            string oldName = _customerBeingEdited.Name;
            string newName = name;

            _customerBeingEdited.Name = name;
            _customerBeingEdited.Email = email;

            repo.UpdateCustomer(_customerBeingEdited);

            // Rename folder if customer name changed
            string oldFolderPath = PathHelper.GetCustomerFolderPathOnly(_editor, oldName);
            string newFolderPath = PathHelper.GetCustomerFolderPathOnly(_editor, newName);

            if (Directory.Exists(oldFolderPath) && !Directory.Exists(newFolderPath))
            {
                Directory.Move(oldFolderPath, newFolderPath);
            }

            _customerBeingEdited = null;
            AddButton.Content = "Add Customer";
        }
        else
        {
            // We're adding a new customer
            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Month = PathHelper.GetMonth(),
                Date = PathHelper.GetDay(),
                Editor = _editor,
                Name = name,
                Email = email
            };

            repo.AddCustomer(customer);
            PathHelper.GetCustomerFolder(_editor, customer.Name);
        }

        // Clear input fields
        NameTextBox.Text = "";
        EmailTextBox.Text = "";

        // Refresh customer list
        LoadCustomers();

        var successDialog = new ContentDialog
        {
            Title = "Success",
            Content = "Customer saved successfully.",
            CloseButtonText = "OK",
            XamlRoot = NameTextBox.XamlRoot
        };
        await successDialog.ShowAsync();
    }

    private void EmailTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            AddCustomer_Click(sender, new RoutedEventArgs());
        }
    }

    private void EditCustomer_Click(object sender, RoutedEventArgs e)
    {
        if (CustomerDataGrid.SelectedItem is Customer selectedCustomer)
        {
            _customerBeingEdited = selectedCustomer;

            // Fill input fields with selected customer's data
            NameTextBox.Text = selectedCustomer.Name;
            EmailTextBox.Text = selectedCustomer.Email;

            // Change the Add button to "Update"
            AddButton.Content = "Update Customer";
        }
        else
        {
            var dialog = new ContentDialog
            {
                Title = "No Selection",
                Content = "Please select a customer to edit.",
                CloseButtonText = "OK",
                XamlRoot = NameTextBox.XamlRoot
            };
            _ = dialog.ShowAsync();
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

    private async void DeleteCustomer_Click(object sender, RoutedEventArgs e)
    {
        if (CustomerDataGrid.SelectedItem is Customer selectedCustomer)
        {
            var dialog = new ContentDialog
            {
                Title = "Confirm Delete",
                Content = $"Are you sure you want to delete {selectedCustomer.Name}?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                XamlRoot = NameTextBox.XamlRoot // Required in WinUI 3
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var repo = new CustomerRepository(PathHelper.GetUserDbPath(_editor));
                repo.DeleteCustomer(selectedCustomer.Id);
                LoadCustomers(); // Refresh the table
            }
        }
        else
        {
            var noSelectionDialog = new ContentDialog
            {
                Title = "No Selection",
                Content = "Please select a customer to delete.",
                CloseButtonText = "OK",
                XamlRoot = NameTextBox.XamlRoot
            };

            await noSelectionDialog.ShowAsync();
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