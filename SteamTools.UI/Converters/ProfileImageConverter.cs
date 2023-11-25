using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SteamTools.Domain.Models;

namespace SteamTools.UI.Converters;

public class ProfileImageConverter : IValueConverter
{
    private readonly BitmapImage _placeholderImage;

    public ProfileImageConverter()
    {
        var path = new Uri("pack://application:,,,/Resources/placeholder.jpg");
        _placeholderImage = new BitmapImage(path);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SteamProfile steamProfile) return _placeholderImage;

        if (steamProfile.PlayerSummaries is null) return _placeholderImage;

        return string.IsNullOrWhiteSpace(steamProfile.PlayerSummaries.AvatarFull)
            ? _placeholderImage
            : new BitmapImage(new Uri(steamProfile.PlayerSummaries.AvatarFull));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException(); // skipcq: CS-A1003
    }
}