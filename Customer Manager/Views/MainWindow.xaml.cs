using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public MainWindow(string editor)
    {
        this.InitializeComponent();
        _editor = editor;
    }

    private async void AddCustomer_Click(object sender, RoutedEventArgs e)
    {
        AddButton.IsEnabled = false; // Prevent double submission

        try
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
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Month = PathHelper.GetMonth(),
                Date = PathHelper.GetDay(),
                Editor = _editor,
                Name = name,
                Email = email
            };

            // Save to user's database
            string dbPath = PathHelper.GetUserDbPath(_editor);
            var repo = new CustomerRepository(dbPath);
            repo.AddCustomer(customer);

            // Create folder in NAS for customer
            PathHelper.GetCustomerFolder(_editor, customer.Name);

            // Clear input fields
            NameTextBox.Text = "";
            EmailTextBox.Text = "";

            var successDialog = new ContentDialog
            {
                Title = "Success",
                Content = "Customer added successfully.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await successDialog.ShowAsync();
        }
        finally
        {
            AddButton.IsEnabled = true; // Re-enable after operation
        }
    }

    private void EmailTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            AddCustomer_Click(AddButton, new RoutedEventArgs());
        }
    }
}