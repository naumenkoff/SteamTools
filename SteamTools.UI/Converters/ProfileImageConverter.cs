using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SteamTools.ProfileDataFetcher.Models;

namespace SteamTools.UI.Converters;

public class ProfileImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SteamProfile steamProfile) return GetPlaceholderImage();
        if (steamProfile.PlayerSummaries is null) return GetPlaceholderImage();
        return string.IsNullOrWhiteSpace(steamProfile.PlayerSummaries.AvatarFull)
            ? GetPlaceholderImage()
            : new BitmapImage(new Uri(steamProfile.PlayerSummaries.AvatarFull));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static BitmapImage GetPlaceholderImage()
    {
        var path = new Uri("pack://application:,,,/Resources/placeholder.jpg");
        return new BitmapImage(path);
    }
}