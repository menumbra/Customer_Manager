using System;
using System.IO;
using Customer_Manager.Helpers;

namespace Customer_Manager.Helpers;

public static class PathHelper
{
    public static string GetMonth() => DateTime.Now.ToString("MMMM");
    public static string GetDay() => DateTime.Now.Day.ToString();

    public static string GetUserDbPath(string editor)
    {
        var basePath = AppSettings.GetBaseFolderPath();

        if (string.IsNullOrEmpty(basePath))
        {
            throw new InvalidOperationException("Base folder path not set. Go to Settings to pick one.");
        }

        string path = Path.Combine(basePath, GetMonth(), GetDay(), editor);
        Directory.CreateDirectory(path);
        return Path.Combine(path, "customer.db");
    }

    public static string GetMainDbPath()
    {
        var basePath = AppSettings.GetBaseFolderPath();

        if (string.IsNullOrEmpty(basePath))
        {
            throw new InvalidOperationException("Base folder path not set. Go to Settings to pick one.");
        }

        string path = Path.Combine(basePath, GetMonth(), GetDay());
        Directory.CreateDirectory(path);
        return Path.Combine(path, "main.db");
    }

    // ✅ Used for rename, delete, open
    public static string GetCustomerFolderPathOnly(string editor, string customerName)
    {
        var basePath = AppSettings.GetBaseFolderPath();

        if (string.IsNullOrEmpty(basePath))
        {
            throw new InvalidOperationException("Base folder path not set. Go to Settings to pick a folder.");
        }

        return Path.Combine(basePath, GetMonth(), GetDay(), editor, customerName);
    }

    // ✅ Used on customer add
    public static string GetCustomerFolder(string editor, string customerName)
    {
        string basePath = GetCustomerFolderPathOnly(editor, customerName);

        Directory.CreateDirectory(basePath); // main customer folder

        string photosPath = Path.Combine(basePath, "Photos");
        string videosPath = Path.Combine(basePath, "Videos");

        Directory.CreateDirectory(photosPath);
        Directory.CreateDirectory(videosPath);

        return basePath;
    }
}
