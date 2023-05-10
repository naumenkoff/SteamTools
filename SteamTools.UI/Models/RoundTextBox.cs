using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SteamTools.UI.Models;

public class RoundTextBox : TextBox
{
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundTextBox));

    public static readonly DependencyProperty PlaceholderTextProperty =
        DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(RoundTextBox));

    public static readonly DependencyProperty PlaceholderForegroundProperty =
        DependencyProperty.Register(nameof(PlaceholderForeground), typeof(Brush), typeof(RoundTextBox));

    public static readonly DependencyProperty SearchButtonCommandProperty = DependencyProperty.Register(
        nameof(SearchButtonCommand),
        typeof(ICommand), typeof(RoundTextBox), new FrameworkPropertyMetadata((ICommand)null));

    public static readonly DependencyProperty SearchButtonCommandParameterProperty =
        DependencyProperty.Register(nameof(SearchButtonCommandParameter), typeof(object), typeof(RoundTextBox),
            new FrameworkPropertyMetadata((object)null));

    public static readonly DependencyProperty ClearButtonShouldHideProperty =
        DependencyProperty.Register(nameof(ClearButtonShouldHide), typeof(bool), typeof(RoundTextBox),
            new FrameworkPropertyMetadata(true));

    public static readonly DependencyProperty ClearButtonCommandProperty =
        DependencyProperty.Register(nameof(ClearButtonCommand), typeof(ICommand), typeof(RoundTextBox),
            new FrameworkPropertyMetadata((ICommand)null));

    public static readonly DependencyProperty ClearButtonCommandParameterProperty =
        DependencyProperty.Register(nameof(ClearButtonCommandParameter), typeof(object), typeof(RoundTextBox),
            new FrameworkPropertyMetadata((object)null));

    public static readonly DependencyProperty ClearButtonVisibilityProperty =
        DependencyProperty.Register(nameof(ClearButtonVisibility), typeof(Visibility), typeof(RoundTextBox));

    public static readonly DependencyProperty SearchButtonVisibilityProperty =
        DependencyProperty.Register(nameof(SearchButtonVisibility), typeof(Visibility), typeof(RoundTextBox));

    public static readonly DependencyProperty ClearButtonUsesBehaviorProperty =
        DependencyProperty.Register(nameof(ClearButtonUsesBehavior), typeof(bool), typeof(RoundTextBox),
            new FrameworkPropertyMetadata(true));


    /// <summary>
    ///     Determines whether the clear button should be hidden when the textbox is empty.
    /// </summary>
    public bool ClearButtonShouldHide
    {
        get => (bool)GetValue(ClearButtonShouldHideProperty);
        set => SetValue(ClearButtonShouldHideProperty, value);
    }

    /// <summary>
    ///     Determines whether the clear button should clear the contents of the textbox.
    /// </summary>
    public bool ClearButtonUsesBehavior
    {
        get => (bool)GetValue(ClearButtonUsesBehaviorProperty);
        set => SetValue(ClearButtonUsesBehaviorProperty, value);
    }

    /// <summary>
    ///     The parameter to pass to the clear button's command.
    /// </summary>
    public object ClearButtonCommandParameter
    {
        get => GetValue(ClearButtonCommandParameterProperty);
        set => SetValue(ClearButtonCommandParameterProperty, value);
    }

    /// <summary>
    ///     The command to execute when the clear button is clicked.
    /// </summary>
    public ICommand ClearButtonCommand
    {
        get => (ICommand)GetValue(ClearButtonCommandProperty);
        set => SetValue(ClearButtonCommandProperty, value);
    }

    /// <summary>
    ///     The parameter to pass to the search button's command.
    /// </summary>
    public object SearchButtonCommandParameter
    {
        get => GetValue(SearchButtonCommandParameterProperty);
        set => SetValue(SearchButtonCommandParameterProperty, value);
    }

    /// <summary>
    ///     Determines the visibility of the cancel button.
    /// </summary>
    public Visibility ClearButtonVisibility
    {
        get => (Visibility)GetValue(ClearButtonVisibilityProperty);
        set => SetValue(ClearButtonVisibilityProperty, value);
    }

    /// <summary>
    ///     Determines the visibility of the search button.
    /// </summary>
    public Visibility SearchButtonVisibility
    {
        get => (Visibility)GetValue(SearchButtonVisibilityProperty);
        set => SetValue(SearchButtonVisibilityProperty, value);
    }

    /// <summary>
    ///     The command to execute when the search button is clicked.
    /// </summary>
    public ICommand SearchButtonCommand
    {
        get => (ICommand)GetValue(SearchButtonCommandProperty);
        set => SetValue(SearchButtonCommandProperty, value);
    }

    /// <summary>
    ///     The corner radius of the TextBox.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    ///     The text to display as a placeholder when the TextBox is empty.
    /// </summary>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    ///     The color of the placeholder text.
    /// </summary>
    public Brush PlaceholderForeground
    {
        get => (Brush)GetValue(PlaceholderForegroundProperty);
        set => SetValue(PlaceholderForegroundProperty, value);
    }
}