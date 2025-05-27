using Windows.Storage;

namespace Customer_Manager.Helpers;

public static class AppSettings
{
    private const string BaseFolderKey = "BaseFolderPath";

    public static void SetBaseFolderPath(string path)
    {
        ApplicationData.Current.LocalSettings.Values[BaseFolderKey] = path;
    }

    public static string? GetBaseFolderPath()
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(BaseFolderKey, out object? value))
        {
            if (value is string path)
            {
                return path;
            }
        }

        return null;
    }
}
