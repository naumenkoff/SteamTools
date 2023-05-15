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
        var size = value switch
        {
            double doubleValue => doubleValue,
            string stringValue when double.TryParse(stringValue, out var doubleValue) => doubleValue,
            _ => 0
        };

        return size / 2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}