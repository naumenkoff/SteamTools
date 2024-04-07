using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Xaml.Behaviors;

// ReSharper disable All

namespace SteamTools.Presentation.Behaviors;

public class WindowBehavior : Behavior<Window>
{
    private const int Windows11 = 22000;

    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    private static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute,
        ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
        uint cbAttribute);

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += OnLoaded;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Environment.OSVersion.Version.Major < 10 || Environment.OSVersion.Version.Build < Windows11)
        {
            return;
        }

        var window = Window.GetWindow(AssociatedObject);
        if (window is null) return;

        var hWnd = new WindowInteropHelper(window).EnsureHandle();
        var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
        DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref preference, sizeof(uint));
    }

    private enum DWM_WINDOW_CORNER_PREFERENCE
    {
        /// <summary>
        ///     Let the system decide whether or not to round window corners.
        /// </summary>
        DWMWCP_DEFAULT = 0,

        /// <summary>
        ///     Never round window corners.
        /// </summary>
        DWMWCP_DONOTROUND = 1,

        /// <summary>
        ///     Round the corners if appropriate.
        /// </summary>
        DWMWCP_ROUND = 2,

        /// <summary>
        ///     Round the corners if appropriate, with a small radius.
        /// </summary>
        DWMWCP_ROUNDSMALL = 3
    }

    private enum DWMWINDOWATTRIBUTE
    {
        DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }
}