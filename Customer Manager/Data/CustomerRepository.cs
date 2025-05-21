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
}
