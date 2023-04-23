using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SteamTools.UI.Converters;

public class HeightToMarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double thickness) return new Thickness(0);
        if (parameter is not string position) return new Thickness(0);
        return position switch
        {
            "Left" => new Thickness(thickness, 0, 0, 0),
            "Right" => new Thickness(0, 0, thickness, 0),
            "Top" => new Thickness(0, thickness, 0, 0),
            "Bottom" => new Thickness(0, 0, 0, thickness),
            "LeftRight" => new Thickness(thickness * 2.5, 0, thickness * 1.5, 0),
            "TopBottom" => new Thickness(0, thickness * 2.5, 0, thickness * 1.5),
            _ => new Thickness(0)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}