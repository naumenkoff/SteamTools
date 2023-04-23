using System.Windows;
using System.Windows.Controls;

namespace SteamTools.UI.Models;

public class RoundButton : Button
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundButton));


    public static readonly DependencyProperty TextAlignmentProperty =
        DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(RoundButton));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }
}