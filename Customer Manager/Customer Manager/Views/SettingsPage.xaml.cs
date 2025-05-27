using Customer_Manager.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Customer_Manager;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Customer_Manager.Views;
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();

        // ✅ Load and show the saved folder path
        var savedPath = Helpers.AppSettings.GetBaseFolderPath();
        if (!string.IsNullOrEmpty(savedPath))
        {
            SelectedFolderPathText.Text = savedPath;
        }
    }

    private void ThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeSelector.SelectedItem is ComboBoxItem selectedItem &&
            selectedItem.Tag is string themeTag)
        {
            AppSettings.SetThemePreference(themeTag);

            if (App.AppWindowInstance?.Content is FrameworkElement root)
            {
                root.RequestedTheme = themeTag switch
                {
                    "Light" => ElementTheme.Light,
                    "Dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };
            }
        }
    }

    private async void PickFolderButton_Click(object sender, RoutedEventArgs e)
    {
        var folderPicker = new FolderPicker();

        // This works reliably if your Shell window is still App.AppWindowInstance
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.AppWindowInstance);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        folderPicker.FileTypeFilter.Add("*");

        var folder = await folderPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            SelectedFolderPathText.Text = folder.Path;
            Helpers.AppSettings.SetBaseFolderPath(folder.Path);
        }
        else
        {
            SelectedFolderPathText.Text = "No folder selected.";
        }
    }

}
