using Microsoft.UI;
using Microsoft.UI.Windowing;
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
using Windows.Graphics;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Customer_Manager.Views;

public sealed partial class Shell : Page
{
    public TextBlock HeaderTotal => HeaderTotalText;
    public TextBlock HeaderIg => HeaderIgText;
    public TextBlock HeaderHc => HeaderHcText;

    private readonly string _editor;
    public string UserDisplayName { get; set; } = "Signed in as ?";
    public string UserInitials { get; set; } = "??";



    public Shell(string editor)
    {
        this.InitializeComponent();

        NavView.PaneOpened += (s, e) => UpdateUserBlockVisibility(true);
        NavView.PaneClosed += (s, e) => UpdateUserBlockVisibility(false);
        UpdateUserBlockVisibility(NavView.IsPaneOpen); // force initial check


        _editor = editor;
        UserDisplayName = $"Signed in as {_editor}";
        UserInitials = GetInitials(_editor);

        // Set editor context
        CustomerPage.CurrentEditor = _editor;

        var cp = new CustomerPage
        {
            HeaderTotal = HeaderTotal,
            HeaderIg = HeaderIg,
            HeaderHc = HeaderHc
        };

        ContentFrame.Navigate(typeof(CustomerPage));

        DispatcherQueue.TryEnqueue(() =>
        {
            if (ContentFrame.Content is CustomerPage cp)
            {
                cp.HeaderTotal = HeaderTotal;
                cp.HeaderIg = HeaderIg;
                cp.HeaderHc = HeaderHc;
                cp.UpdateCounters();
            }
        });

        // Setup Fluent window size
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.AppWindowInstance);
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new SizeInt32(900, 600));

        // 🪟 Fluent Title Bar Customization
        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        var isDark = Application.Current.RequestedTheme == ApplicationTheme.Dark;
        appWindow.TitleBar.ButtonForegroundColor = isDark ? Colors.White : Colors.Black;

        // Make NavView the draggable region
        var window = App.AppWindowInstance;
        window.SetTitleBar(NavView); // ✅ CORRECT

    }

    private void Shell_Loaded(object sender, RoutedEventArgs e)
    {
        // Navigate only after UI is loaded
        ContentFrame.Navigate(typeof(CustomerPage));
    }


    private void UpdateUserBlockVisibility(bool isPaneOpen)
    {
        if (UserBlockItem != null)
        {
            UserBlockItem.Visibility = isPaneOpen ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "??";

        var parts = name.Split(new[] { ' ', '.', '_' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpper();
        return string.Concat(parts[0][0], parts[^1][0]).ToUpper();
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            switch (item.Tag)
            {
                case "customers":
                    ContentFrame.Navigate(typeof(CustomerPage));
                    break;
                case "settings":
                    ContentFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }

        if (ContentFrame.Content is CustomerPage cp)
        {
            // Ensure counts are up-to-date
            cp.UpdateCounters();
            // Update header text with current counts
            HeaderTotalText.Text = $"Total: {cp.TotalCount}";
            HeaderIgText.Text = $"IG: {cp.IgCount}";
            HeaderHcText.Text = $"HC: {cp.HcCount}";
        }
    }

    private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer == LogoutItem)
        {
            var confirmDialog = new ContentDialog
            {
                Title = "Log Out",
                Content = "Are you sure you want to log out?",
                PrimaryButtonText = "Log out",
                CloseButtonText = "Cancel",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await confirmDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var login = new LoginWindow();
                login.Activate();
                App.AppWindowInstance.Close();
            }
        }
    }

}
