using System.Windows;
using System.Windows.Controls.Primitives;

namespace SteamTools.UI.Resources;

public class TrackRepeatButton : RepeatButton
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TrackRepeatButton));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
}