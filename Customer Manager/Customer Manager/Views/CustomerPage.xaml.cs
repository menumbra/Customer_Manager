using Customer_Manager.Data;
using Customer_Manager.Helpers;
using Customer_Manager.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Customer_Manager.Views;

public sealed partial class CustomerPage : Page
{
    private readonly string _editor;
    private ObservableCollection<Customer> _customers = new();

    public int TotalCount { get; private set; }
    public int IgCount { get; private set; }
    public int HcCount { get; private set; }

    public TextBlock? HeaderTotal { get; set; }
    public TextBlock? HeaderIg { get; set; }
    public TextBlock? HeaderHc { get; set; }


    public static string CurrentEditor { get; set; } = ""; // Set from Shell
    public CustomerPage()
    {
        this.InitializeComponent();
        _editor = CurrentEditor;
        this.SizeChanged += CustomerPage_SizeChanged;
        LoadCustomers();
    }

    private void CustomerPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        double width = e.NewSize.Width;

        // Hide email if window is narrower than 640px
        if (EmailColumn != null)
            EmailColumn.Visibility = width < 640 ? Visibility.Collapsed : Visibility.Visible;
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        UpdateCounters(); // ✅ forces header update when page is shown
    }

    public void UpdateCounters()
    {
        TotalCount = _customers.Count;
        IgCount = _customers.Count(c => c.SME == "IG");
        HcCount = _customers.Count(c => c.SV == "HC");

        // ✅ Update NavigationView header if bound
        if (HeaderTotal != null)
            HeaderTotal.Text = $"Total: {TotalCount}";
        if (HeaderIg != null)
            HeaderIg.Text = $"IG: {IgCount}";
        if (HeaderHc != null)
            HeaderHc.Text = $"HC: {HcCount}";

    }

    private void LoadCustomers()
    {
        string dbPath = PathHelper.GetUserDbPath(_editor);
        var repo = new CustomerRepository(dbPath);
        var customers = repo.GetCustomers();

        _customers = new ObservableCollection<Customer>(customers);
        CustomerDataGrid.ItemsSource = _customers;

        UpdateCounters();
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

            var repo = new CustomerRepository(PathHelper.GetUserDbPath(_editor));
            repo.AddCustomer(customer);

            // ✅ Handle missing folder path safely
        try
        {
            PathHelper.GetCustomerFolder(_editor, customer.Name);
        }
        catch (InvalidOperationException ex)
        {
            var warning = new ContentDialog
            {
                Title = "Missing Folder Setting",
                Content = ex.Message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await warning.ShowAsync();
        }

            PathHelper.GetCustomerFolder(_editor, customer.Name);
            LoadCustomers();
            UpdateCounters(); // Update counters after adding a new customer
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadCustomers();
        UpdateCounters(); // Update counters after refresh
    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is Customer customer)
        {
            string folderPath = PathHelper.GetCustomerFolderPathOnly(_editor, customer.Name);
            if (Directory.Exists(folderPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
            }
        }
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
                    Directory.Delete(folderPath, true);
                }

                LoadCustomers();
                UpdateCounters(); // Update counters after deletion
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
                    try
                    {
                        string oldPath = PathHelper.GetCustomerFolderPathOnly(_editor, oldName);
                        string newPath = PathHelper.GetCustomerFolderPathOnly(_editor, customer.Name);

                        if (Directory.Exists(oldPath) && !Directory.Exists(newPath))
                        {
                            Directory.Move(oldPath, newPath);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        var warning = new ContentDialog
                        {
                            Title = "Folder Rename Error",
                            Content = ex.Message,
                            CloseButtonText = "OK",
                            XamlRoot = this.Content.XamlRoot
                        };

                        await warning.ShowAsync();
                    }
                }

                LoadCustomers();
            }
        }
    }
}
