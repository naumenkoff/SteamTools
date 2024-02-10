using System;
using System.Globalization;
using System.Windows.Data;

namespace SteamTools.UI.Converters;

/// <summary>
///     Converter to calculate the size of Thumb equal to half the size of the Track for a ScrollBar.
/// </summary>
public class ThumbHalfSizeConverter : IValueConverter
{
    /// <summary>
    ///     Converts the specified value to half of itself.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue) return doubleValue / 2.0;
        if (value is string stringValue && double.TryParse(stringValue, out doubleValue)) return doubleValue / 2.0;
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}