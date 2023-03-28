using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamTools.UI.Services.Navigation;

public interface INavigationService
{
    ObservableObject CurrentView { get; }
    void Navigate<T>() where T : ObservableObject;
}