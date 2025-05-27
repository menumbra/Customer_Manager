using Customer_Manager.Helpers;
using Microsoft.UI.Xaml;
using System;

namespace Customer_Manager
{
    public partial class App : Application
    {
        public static Window? AppWindowInstance { get; set; }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var loginWindow = new Views.LoginWindow();
            AppWindowInstance = loginWindow;
            loginWindow.Activate();

            var themePref = AppSettings.GetThemePreference();
            if (AppWindowInstance?.Content is FrameworkElement root)
            {
                root.RequestedTheme = themePref switch
                {
                    "Light" => ElementTheme.Light,
                    "Dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default
                };
            }
        }
    }
}
