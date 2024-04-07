using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SteamTools.Presentation.Resources;

public class HorizontalSlider : Slider
{
    public static readonly DependencyProperty TrackHeightProperty =
        DependencyProperty.Register(nameof(TrackHeight), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackRadiusXProperty =
        DependencyProperty.Register(nameof(TrackRadiusX), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackRadiusYProperty =
        DependencyProperty.Register(nameof(TrackRadiusY), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackLeftBackgroundProperty =
        DependencyProperty.Register(nameof(TrackLeftBackground), typeof(Brush), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackLeftRadiusProperty =
        DependencyProperty.Register(nameof(TrackLeftRadius), typeof(CornerRadius), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackRightBackgroundProperty =
        DependencyProperty.Register(nameof(TrackRightBackground), typeof(Brush), typeof(HorizontalSlider));

    public static readonly DependencyProperty TrackRightRadiusProperty =
        DependencyProperty.Register(nameof(TrackRightRadius), typeof(CornerRadius), typeof(HorizontalSlider));

    public static readonly DependencyProperty ThumbHeightProperty =
        DependencyProperty.Register(nameof(ThumbHeight), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty ThumbWidthProperty =
        DependencyProperty.Register(nameof(ThumbWidth), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty ThumbRadiusXProperty =
        DependencyProperty.Register(nameof(ThumbRadiusX), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty ThumbRadiusYProperty =
        DependencyProperty.Register(nameof(ThumbRadiusY), typeof(double), typeof(HorizontalSlider));

    public static readonly DependencyProperty ThumbBrushProperty =
        DependencyProperty.Register(nameof(ThumbBrush), typeof(Brush), typeof(HorizontalSlider));

    public double TrackHeight
    {
        get => (double)GetValue(TrackHeightProperty);
        set => SetValue(TrackHeightProperty, value);
    }

    public double TrackRadiusX
    {
        get => (double)GetValue(TrackRadiusXProperty);
        set => SetValue(TrackRadiusXProperty, value);
    }

    public double TrackRadiusY
    {
        get => (double)GetValue(TrackRadiusYProperty);
        set => SetValue(TrackRadiusYProperty, value);
    }

    public Brush TrackLeftBackground
    {
        get => (Brush)GetValue(TrackLeftBackgroundProperty);
        set => SetValue(TrackLeftBackgroundProperty, value);
    }

    public CornerRadius TrackLeftRadius
    {
        get => (CornerRadius)GetValue(TrackLeftRadiusProperty);
        set => SetValue(TrackLeftRadiusProperty, value);
    }

    public Brush TrackRightBackground
    {
        get => (Brush)GetValue(TrackRightBackgroundProperty);
        set => SetValue(TrackRightBackgroundProperty, value);
    }

    public CornerRadius TrackRightRadius
    {
        get => (CornerRadius)GetValue(TrackRightRadiusProperty);
        set => SetValue(TrackRightRadiusProperty, value);
    }

    public double ThumbHeight
    {
        get => (double)GetValue(ThumbHeightProperty);
        set => SetValue(ThumbHeightProperty, value);
    }

    public double ThumbWidth
    {
        get => (double)GetValue(ThumbWidthProperty);
        set => SetValue(ThumbWidthProperty, value);
    }

    public double ThumbRadiusX
    {
        get => (double)GetValue(ThumbRadiusXProperty);
        set => SetValue(ThumbRadiusXProperty, value);
    }

    public double ThumbRadiusY
    {
        get => (double)GetValue(ThumbRadiusYProperty);
        set => SetValue(ThumbRadiusYProperty, value);
    }

    public Brush ThumbBrush
    {
        get => (Brush)GetValue(ThumbBrushProperty);
        set => SetValue(ThumbBrushProperty, value);
    }
}