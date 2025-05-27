using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Customer_Manager.Data;

public static class UserRepository
{
    private static string GetDbPath()
    {
        string nasPath = @"E:\NAS\May\21\main.db"; // Example path
        Directory.CreateDirectory(Path.GetDirectoryName(nasPath)!);
        return nasPath;
    }

    public static void Initialize()
    {
        using var connection = new SqliteConnection($"Data Source={GetDbPath()}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Users (
                Username TEXT PRIMARY KEY,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL
            );
        """;
        command.ExecuteNonQuery();
    }

    public static bool ValidateUser(string username, string password, out string role)
    {
        using var connection = new SqliteConnection($"Data Source={GetDbPath()}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Role FROM Users WHERE Username = $u AND Password = $p";
        command.Parameters.AddWithValue("$u", username);
        command.Parameters.AddWithValue("$p", password);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            role = reader.GetString(0);
            return true;
        }

        role = "";
        return false;
    }

    public static void SeedUsers()
    {
        using var connection = new SqliteConnection($"Data Source={GetDbPath()}");
        connection.Open();

        var insert = connection.CreateCommand();
        insert.CommandText = """
            INSERT OR IGNORE INTO Users (Username, Password, Role) VALUES
            ('admin', 'admin123', 'admin'),
            ('user1', 'user123', 'user');
        """;
        insert.ExecuteNonQuery();
    }
}
