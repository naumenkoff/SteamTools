using System.Windows;
using System.Windows.Controls;

namespace SteamTools.UI.Models;

public class StackPanelWithSpacing : StackPanel
{
    public static readonly DependencyProperty SpacingProperty =
        DependencyProperty.Register(nameof(Spacing), typeof(Thickness), typeof(StackPanelWithSpacing),
            new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsArrange));

    public Thickness Spacing
    {
        get => (Thickness)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
        double offset = 0;

        foreach (UIElement child in InternalChildren)
        {
            if (child.Visibility == Visibility.Collapsed)
                continue;

            var top = offset + Spacing.Top;
            var bottom = offset + child.DesiredSize.Height + Spacing.Bottom;

            child.Arrange(new Rect(new Point(Spacing.Left, top),
                new Point(arrangeSize.Width - Spacing.Right, bottom)));
            offset = bottom;
        }

        return arrangeSize;
    }
}