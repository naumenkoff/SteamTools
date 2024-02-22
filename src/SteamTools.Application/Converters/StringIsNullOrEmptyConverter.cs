using System;
using System.Globalization;
using System.Windows.Data;

namespace SteamTools.Presentation.Converters;

public class StringIsNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not string text || string.IsNullOrEmpty(text);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}