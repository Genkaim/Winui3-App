using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace randomtest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Haowen : Page
    {
        public Haowen()
        {
            this.InitializeComponent();
        }
        private void Show(object sender, TappedRoutedEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                // 创建一个新的 TranslateTransform 并将其应用到 Border
                TranslateTransform transform = new TranslateTransform();
                border.RenderTransform = transform;

                // 创建一个新的动画来改变 Border 的 Opacity 属性
                DoubleAnimation opacityAnimation = new DoubleAnimation();

                // 判断 Border 的 Opacity 属性
                if (border.Opacity > 0.01)
                {
                    opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
                    opacityAnimation.From = border.Opacity;
                    opacityAnimation.To = 0.01;
                }
                else
                {
                    opacityAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
                    opacityAnimation.From = border.Opacity;
                    opacityAnimation.To = 1.0;
                }

                Storyboard.SetTarget(opacityAnimation, border);
                Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

                // 创建一个新的 Storyboard 并开始动画
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(opacityAnimation);
                storyboard.Begin();
            }
        }

    }
}
