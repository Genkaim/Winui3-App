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
using muxc = Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace randomtest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {

        public BlankPage1()
        {
            this.InitializeComponent();
            contentFrame.Navigate(typeof(randomtest.BlankPage3));
            this.Loaded += BlankPage1_Loaded;
        }
        private void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            // �ҵ�"��ҳ"�������������Ϊѡ����
            foreach (var item in NavigationViewControl.MenuItems)
            {
                if (item is muxc.NavigationViewItem navItem && (string)navItem.Content == "��ҳ")
                {
                    NavigationViewControl.SelectedItem = item;
                    break;
                }
            }
        }

        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            // �����������Ĵ���
        }
        private void NavigationViewControl_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            var invokedItem = args.InvokedItem as string;
            if (invokedItem == "��ҳ")
            {
                contentFrame.Navigate(typeof(randomtest.BlankPage3));

            }
            else if (invokedItem == "��")
            {
                contentFrame.Navigate(typeof(randomtest.BlankPage2));
            }
            /*
            else if (invokedItem == "�ٺ�")
            {
                contentFrame.Navigate(typeof(randomtest.Haowen));
            }*/
            var navItemTag = args.InvokedItemContainer.Tag.ToString();
            if (navItemTag == "Settings")
            {
                contentFrame.Navigate(typeof(randomtest.Settings));
                // �����������Ĵ�������������ͼ��ĵ���¼�
            }
        }

    }

}
