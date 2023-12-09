using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using static randomtest.BlankPage3;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace randomtest
{
    public static class GlobalData
    {
        public static DataWithIndex[] Data { get; set; }
        public static bool AllowDuplicates { get; set; }
    }

    public sealed partial class Settings : Page
    {

        private bool isPageLoading = true;

        public Settings()
        { 
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("AppTheme"))
            {
                var theme = localSettings.Values["AppTheme"].ToString();
                foreach (ComboBoxItem item in ThemeComboBox.Items)
                {
                    if (item.Content.ToString() == theme)
                    {
                        ThemeComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                ThemeComboBox.SelectedItem = ThemeComboBox.Items[0];
            }

            // Check if "AllowDuplicates" key exists in local settings  
            if (localSettings.Values.ContainsKey("AllowDuplicates"))
            {
                // Get the value of "AllowDuplicates" key  
                var allowDuplicates = Convert.ToBoolean(localSettings.Values["AllowDuplicates"]);
                // Set the IsOn property of MyToggleSwitch based on the value of allowDuplicates  
                MyToggleSwitch.IsOn = allowDuplicates;
                GlobalData.AllowDuplicates = MyToggleSwitch.IsOn;
            }
            else
            {
                // If "AllowDuplicates" key does not exist, set MyToggleSwitch to default value (false)  
                MyToggleSwitch.IsOn = false;
            }
            isPageLoading = false;
        }

        private async void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isPageLoading) return;
            var selectedTheme = ((ComboBoxItem)ThemeComboBox.SelectedItem).Content.ToString();
            ApplicationData.Current.LocalSettings.Values["AppTheme"] = selectedTheme;
            ContentDialog Dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = "提示",
                Content = "重启APP以应用更改",
                CloseButtonText = "确认"
            };

            ContentDialogResult result = await Dialog.ShowAsync();
        }

        private void MyToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            // 更新全局变量的值
            GlobalData.AllowDuplicates  = MyToggleSwitch.IsOn;
            ApplicationData.Current.LocalSettings.Values["AllowDuplicates"] = GlobalData.AllowDuplicates ;
        }
    }
}
