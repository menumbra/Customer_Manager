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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Customer_Manager.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        LoadSavedPath();
    }

    private async void ChooseFolder_Click(object sender, RoutedEventArgs e)
    {
        FolderPicker picker = new FolderPicker();
        picker.FileTypeFilter.Add("*");

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        StorageFolder folder = await picker.PickSingleFolderAsync();
        if (folder != null)
        {
            ApplicationData.Current.LocalSettings.Values["BaseFolderPath"] = folder.Path;
            FolderPathText.Text = $"Selected Path: {folder.Path}";
        }
    }

    private void LoadSavedPath()
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue("BaseFolderPath", out object? pathObj))
        {
            string path = pathObj?.ToString() ?? "Not set";
            FolderPathText.Text = $"Saved Path: {path}";
        }
    }

}
