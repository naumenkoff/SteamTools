using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SteamTools.UI.Converters;

public class StringNullOrEmptyToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var text = value as string;
        if (parameter is not null) return string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
        return string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}