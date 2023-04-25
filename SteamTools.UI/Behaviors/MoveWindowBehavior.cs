using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SteamTools.UI.Behaviors;

public class MoveWindowBehavior : Behavior<Grid>
{
    private bool _canMoveApplication;
    private Window _window;
    private bool IsWindowMaximized => _window.WindowState == WindowState.Maximized;

    protected override void OnAttached()
    {
        base.OnAttached();

        _window = Window.GetWindow(AssociatedObject);
        AssociatedObject.MouseMove += OnMouseMove;
        AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
        AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.MouseMove -= OnMouseMove;
        AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
        AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_canMoveApplication is false) return;
        _canMoveApplication = false;
        var point = GetWindowPosition(e);
        SetWindowPosition(point.X - _window.RestoreBounds.Width * 0.5, point.Y);
        ChangeWindowState(WindowState.Normal);
        _window.DragMove();
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ChangeWindowState(IsWindowMaximized ? WindowState.Normal : WindowState.Maximized);
        }
        else
        {
            _canMoveApplication = IsWindowMaximized;
            _window.DragMove();
        }
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _canMoveApplication = false;
    }

    private void ChangeWindowState(WindowState windowState)
    {
        _window.WindowState = windowState;
    }

    private void SetWindowPosition(double x, double y)
    {
        _window.Left = x;
        _window.Top = y;
    }

    private Point GetWindowPosition(MouseEventArgs e)
    {
        var position = e.MouseDevice.GetPosition(_window);
        var point = _window.PointToScreen(position);
        return point;
    }
}