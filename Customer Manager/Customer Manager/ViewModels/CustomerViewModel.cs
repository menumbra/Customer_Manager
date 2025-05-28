using System;
using System.Collections.ObjectModel;
using Customer_Manager.Models;
using Customer_Manager.Helpers;
using Customer_Manager.Data;

namespace Customer_Manager.ViewModels;

public class CustomerViewModel
{
    private readonly CustomerRepository _customerRepository;

    public ObservableCollection<Customer> Customers { get; set; } = new();

    public CustomerViewModel(string dbPath)
    {
        _customerRepository = new CustomerRepository(dbPath);
    }

    public void AddCustomer(Customer newCustomer)
    {
        // Save to database
        _customerRepository.AddCustomer(newCustomer);

        // Create folder
        var folderPath = PathHelper.GetCustomerFolder(newCustomer.Editor, newCustomer.Name);

        // Upload to Dropbox
        var dropboxPath = $"/{newCustomer.Month}/{newCustomer.Date}/{newCustomer.Editor}/{newCustomer.Name}";
        try
        {
            DropboxHelper.UploadFolderAsync(folderPath, dropboxPath).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dropbox upload failed: {ex.Message}");
        }

        // Add to local collection
        Customers.Add(newCustomer);
    }
}
