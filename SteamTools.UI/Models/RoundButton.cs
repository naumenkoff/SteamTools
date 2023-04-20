﻿using System.Windows;
using System.Windows.Controls;

namespace SteamTools.UI.Models;

public class RoundButton : Button
{
    public static readonly DependencyProperty RoundnessProperty =
        DependencyProperty.Register(nameof(Roundness), typeof(CornerRadius), typeof(RoundButton));

    public CornerRadius Roundness
    {
        get => (CornerRadius)GetValue(RoundnessProperty);
        set => SetValue(RoundnessProperty, value);
    }
}