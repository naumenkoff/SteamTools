using System.Windows;
using System.Windows.Controls;

namespace SteamTools.UI.Models;

public class RoundCheckBox : CheckBox
{
    public static readonly DependencyProperty TextAlignmentProperty =
        DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(RoundCheckBox));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundCheckBox));

    public TextAlignment TextAlignment
    {
        get => (TextAlignment) GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius) GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
}