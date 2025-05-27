using Windows.Storage;

namespace Customer_Manager.Helpers;

public static class AppSettings
{
    private const string BaseFolderKey = "BaseFolderPath";

    private const string ThemeKey = "AppTheme";

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

    public static void SetThemePreference(string theme)
    {
        ApplicationData.Current.LocalSettings.Values[ThemeKey] = theme;
    }

    public static string GetThemePreference()
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(ThemeKey, out object value)
            && value is string theme)
        {
            return theme;
        }

        return "Default"; // fallback
    }
}
