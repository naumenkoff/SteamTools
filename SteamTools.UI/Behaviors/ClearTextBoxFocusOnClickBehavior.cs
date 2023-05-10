using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SteamTools.UI.Behaviors;

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
        Application.Current.MainWindow!.MouseLeftButtonUp -= OnMouseLeftButtonUp;
    }

    private static void OnGotFocus(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.MouseLeftButtonUp += OnMouseLeftButtonUp;
    }

    private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
    }
}