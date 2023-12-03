using Microsoft.UI;
using Microsoft.UI.Composition.SystemBackdrops;
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
using WinRT.Interop;
using Microsoft.UI.Windowing;
using PInvoke;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    public const int GWL_STYLE = -16;
    public const int WS_MAXIMIZEBOX = 0x00010000;
    public const int WS_MINIMIZEBOX = 0x00020000;
    public const int WS_SIZEBOX = 0x00040000;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace randomtest
{



    public sealed partial class MainWindow : Window
    {


        private IntPtr hwnd;
        private AppWindow appWindow;
        public MainWindow()
        {
            ExtendsContentIntoTitleBar = true;
            this.InitializeComponent();
            SystemBackdrop = new MicaBackdrop()
            { Kind = MicaKind.Base };
            MyFrame.Navigate(typeof(randomtest.BlankPage1));
            this.InitializeComponent();

            hwnd = WindowNative.GetWindowHandle(this);
            WindowId id = Win32Interop.GetWindowIdFromWindow(hwnd);
            appWindow = AppWindow.GetFromWindowId(id);
            appWindow.MoveAndResize(new RectInt32(_X: 560, _Y: 280, _Width: 1400, _Height: 1040));
            
            int style = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            style &= ~NativeMethods.WS_MAXIMIZEBOX; // 禁止窗口最大化
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, style);
            
            style &= ~NativeMethods.WS_SIZEBOX; // 设置窗体不可调整大小
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, style);
            

        }



    }

}
