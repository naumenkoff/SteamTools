using System.Windows;
using System.Windows.Controls.Primitives;

namespace SteamTools.Presentation.Resources;

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