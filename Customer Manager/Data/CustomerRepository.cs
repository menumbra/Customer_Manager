using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer_Manager.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Customer_Manager.Data;

public class CustomerRepository
{
    private readonly string _dbPath;

    public CustomerRepository(string dbPath)
    {
        _dbPath = dbPath;
        EnsureDatabase();
    }

    private void EnsureDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Execute("""
            CREATE TABLE IF NOT EXISTS Customers (
                Id TEXT PRIMARY KEY,
                Month TEXT,
                Date TEXT,
                Editor TEXT,
                Name TEXT,
                Email TEXT
            )
        """);
    }

    public void AddCustomer(Customer customer)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Execute("""
            INSERT INTO Customers (Id, Month, Date, Editor, Name, Email)
            VALUES (@Id, @Month, @Date, @Editor, @Name, @Email)
        """, customer);
    }

    public List<Customer> GetCustomers()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        return connection.Query<Customer>("SELECT * FROM Customers").ToList();
    }

    //public List<Customer> GetAllCustomers()
    //{
    //    using var connection = new SqliteConnection(_dbPath);
    //    connection.Open();

    //    var customers = new List<Customer>();

    //    var command = connection.CreateCommand();
    //    command.CommandText = "SELECT * FROM Customers";

    //    using var reader = command.ExecuteReader();
    //    while (reader.Read())
    //    {
    //        customers.Add(new Customer
    //        {
    //            Id = reader["Id"]!.ToString(),
    //            Name = reader["Name"]!.ToString(),
    //            Email = reader["Email"]!.ToString(),
    //            Month = reader["Month"]!.ToString(),
    //            Date = reader["Date"]!.ToString(),
    //            Editor = reader["Editor"]!.ToString()
    //        });
    //    }

    //    return customers;
    //}

    public void DeleteCustomer(string id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Execute("DELETE FROM Customers WHERE Id = @Id", new { Id = id });
    }


}
