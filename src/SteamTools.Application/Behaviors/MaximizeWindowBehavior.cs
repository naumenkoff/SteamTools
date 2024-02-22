using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SteamTools.Presentation.Behaviors;

public class MaximizeWindowBehavior : Behavior<Button>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += OnClick;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= OnClick;
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(AssociatedObject)!;
        if (window.WindowState == WindowState.Maximized)
            SystemCommands.RestoreWindow(window);
        else
            SystemCommands.MaximizeWindow(window);
    }
}