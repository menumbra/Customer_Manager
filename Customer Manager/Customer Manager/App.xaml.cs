using Microsoft.UI.Xaml;
using System;

namespace Customer_Manager
{
    public partial class App : Application
    {
        public static Window AppWindowInstance { get; set; } = null!;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var loginWindow = new Views.LoginWindow();
            AppWindowInstance = loginWindow;
            loginWindow.Activate();
        }
    }
}
