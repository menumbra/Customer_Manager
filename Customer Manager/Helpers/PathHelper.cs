using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Manager.Helpers;

public static class PathHelper
{
    private static string baseNas = @"E:\TestFolders"; // NAS base path for testing

    public static string GetMonth() => DateTime.Now.ToString("MMMM");
    public static string GetDay() => DateTime.Now.Day.ToString();

    public static string GetUserDbPath(string editor)
    {
        string path = Path.Combine(baseNas, GetMonth(), GetDay(), editor);
        Directory.CreateDirectory(path);
        return Path.Combine(path, "customer.db");
    }

    public static string GetMainDbPath()
    {
        string path = Path.Combine(baseNas, GetMonth(), GetDay());
        Directory.CreateDirectory(path);
        return Path.Combine(path, "main.db");
    }

    public static string GetCustomerFolder(string editor, string customerName)
    {
        string basePath = GetCustomerFolderPathOnly(editor, customerName);

        // Create the main customer folder
        if (!Directory.Exists(basePath))
        {
            Directory.CreateDirectory(basePath);
        }

        // Create Photos and Videos subfolders
        string photosPath = Path.Combine(basePath, "Photos");
        string videosPath = Path.Combine(basePath, "Videos");

        if (!Directory.Exists(photosPath))
        {
            Directory.CreateDirectory(photosPath);
        }

        if (!Directory.Exists(videosPath))
        {
            Directory.CreateDirectory(videosPath);
        }

        return basePath;
    }


    public static string GetCustomerFolderPathOnly(string editor, string customerName)
    {
        return Path.Combine(baseNas, GetMonth(), GetDay(), editor, customerName);
    }
}
