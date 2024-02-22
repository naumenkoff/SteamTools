using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SteamTools.Presentation.Behaviors;

public class ClearTextBoxFocusOnClickBehavior : Behavior<FrameworkElement>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.GotFocus += OnGotFocus;
        AssociatedObject.LostFocus += OnLostFocus;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.GotFocus -= OnGotFocus;
        AssociatedObject.LostFocus -= OnLostFocus;
    }

    private static void OnLostFocus(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.MouseLeftButtonDown -= OnMouseLeftButtonDown;
    }

    private static void OnGotFocus(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.MouseLeftButtonDown += OnMouseLeftButtonDown;
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
    }
}