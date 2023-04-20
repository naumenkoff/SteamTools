using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SteamTools.UI.Converters;

public class HeightToWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double height) return height;
        return GridLength.Auto;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}