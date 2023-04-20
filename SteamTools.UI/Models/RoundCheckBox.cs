using System.Windows;
using System.Windows.Controls;

namespace SteamTools.UI.Models;

public class RoundCheckBox : CheckBox
{
    public static readonly DependencyProperty TextPositionProperty =
        DependencyProperty.Register(nameof(TextPosition), typeof(TextAlignment), typeof(RoundCheckBox));

    public TextAlignment TextPosition
    {
        get => (TextAlignment)GetValue(TextPositionProperty);
        set => SetValue(TextPositionProperty, value);
    }
}