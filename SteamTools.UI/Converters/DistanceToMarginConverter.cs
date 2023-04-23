using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SteamTools.UI.Converters;

public class DistanceToMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Border border) return new Thickness(0);
        if (border.Child is not TextBlock textBlock || parameter is not string position) return new Thickness(0);

        var distance = border.ActualHeight - textBlock.ActualHeight;
        return position switch
        {
            "Left" => new Thickness(distance, 0, 0, 0),
            "Right" => new Thickness(0, 0, distance, 0),
            "Top" => new Thickness(0, distance, 0, 0),
            "Bottom" => new Thickness(0, 0, 0, distance),
            _ => new Thickness(0)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}