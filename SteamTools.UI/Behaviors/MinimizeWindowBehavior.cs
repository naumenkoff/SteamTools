using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SteamTools.UI.Behaviors;

public class MinimizeWindowBehavior : Behavior<Button>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        var window = Window.GetWindow(AssociatedObject);
        SystemCommands.MinimizeWindow(window);
    }
}