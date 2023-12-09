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
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

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
namespace randomtest
{
    public sealed partial class MainWindow : Window
    {
        private IntPtr hwnd;
        private AppWindow appWindow;

        [DllImport("user32.dll")]
        static extern uint GetDpiForWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const int LOGPIXELSX = 88;
        const int LOGPIXELSY = 90;
        const uint SWP_NOMOVE = 0x0002;
        const uint SWP_NOZORDER = 0x0004;

        public MainWindow()
        {
            this.InitializeComponent();
            hwnd = WindowNative.GetWindowHandle(this);
            WindowId id = Win32Interop.GetWindowIdFromWindow(hwnd);
            appWindow = AppWindow.GetFromWindowId(id);

            ExtendsContentIntoTitleBar = true;
            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.Base };
            MyFrame.Navigate(typeof(randomtest.BlankPage1));

            // 获取窗口的 DPI
            uint dpi = GetDpiForWindow(hwnd);

            // 获取设备上下文
            IntPtr hdc = GetDC(hwnd);

            // 获取显示器的实际分辨率和缩放比例
            int dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
            int dpiY = GetDeviceCaps(hdc, LOGPIXELSY);

            // 根据 DPI 设置窗口大小
            int width = (int)(700.0 * dpiX / 96.0);
            int height = (int)(520.0 * dpiY / 96.0);

            // 设置窗口大小
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, width, height, SWP_NOMOVE | SWP_NOZORDER);

            int style = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            style &= ~NativeMethods.WS_MAXIMIZEBOX; // 禁止窗口最大化
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, style);
            style &= ~NativeMethods.WS_SIZEBOX; // 设置窗体不可调整大小
            NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, style);
        }
    }
}
