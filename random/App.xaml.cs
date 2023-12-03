using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace randomtest
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Data"))
            {
                string jsonData = (string)ApplicationData.Current.LocalSettings.Values["Data"];
                GlobalData.Data = JsonSerializer.Deserialize<DataWithIndex[]>(jsonData);
            }

            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("AppTheme"))
            {
                var theme = localSettings.Values["AppTheme"].ToString();
                switch (theme)
                {
                    case "跟随系统":
                        this.RequestedTheme = Application.Current.RequestedTheme;
                        break;
                    case "黑色主题":
                        this.RequestedTheme = ApplicationTheme.Dark;
                        break;
                    case "白色主题":
                        this.RequestedTheme = ApplicationTheme.Light;
                        break;
                }
            }
            else this.RequestedTheme = Application.Current.RequestedTheme;
        }
        public static Window? Window { get; private set; }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Window = new MainWindow();
            Window.Activate();
        }

        private Window m_window;
    }
}
