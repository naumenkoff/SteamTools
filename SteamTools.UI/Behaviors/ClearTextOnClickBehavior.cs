using System.Windows;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using SteamTools.UI.Models;

namespace SteamTools.UI.Behaviors;

public class ClearTextOnClickBehavior : Behavior<RoundButton>
{
    private RoundTextBox _roundTextBox;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += AssociatedObject_Loaded;
        AssociatedObject.Click += OnClick;
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (_roundTextBox is not null && _roundTextBox.ClearButtonUsesBehavior) _roundTextBox.Text = string.Empty;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= AssociatedObject_Loaded;
        AssociatedObject.Click -= OnClick;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        _roundTextBox = FindTextBox();
    }

    private RoundTextBox FindTextBox()
    {
        var parent = VisualTreeHelper.GetParent(AssociatedObject);

        while (parent != null)
        {
            if (parent is RoundTextBox textBox) return textBox;
            parent = VisualTreeHelper.GetParent(parent);
        }

        return null;
    }
}