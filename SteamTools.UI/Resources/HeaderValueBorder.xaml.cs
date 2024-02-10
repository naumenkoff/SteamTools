using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SteamTools.UI.Resources;

public class HeaderValueBorder : Control
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(nameof(Header), typeof(string), typeof(HeaderValueBorder));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(string), typeof(HeaderValueBorder));

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(HeaderValueBorder));

    public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(nameof(ImageSource), typeof(ImageSource), typeof(HeaderValueBorder));

    public static readonly DependencyProperty ImageVisibilityProperty = DependencyProperty.Register(nameof(ImageVisibility),
        typeof(Visibility),
        typeof(HeaderValueBorder), new FrameworkPropertyMetadata(Visibility.Collapsed));

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand),
        typeof(HeaderValueBorder), new FrameworkPropertyMetadata((ICommand)null));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter),
        typeof(object),
        typeof(HeaderValueBorder), new FrameworkPropertyMetadata((object)null));

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(HeaderValueBorder), new PropertyMetadata(false));

    static HeaderValueBorder()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderValueBorder), new FrameworkPropertyMetadata(typeof(HeaderValueBorder)));
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public Visibility ImageVisibility
    {
        get => (Visibility)GetValue(ImageVisibilityProperty);
        set => SetValue(ImageVisibilityProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        MouseLeftButtonUp += CommandHandler;
    }

    private void CommandHandler(object sender, MouseButtonEventArgs e)
    {
        Command?.Execute(CommandParameter);
    }
}