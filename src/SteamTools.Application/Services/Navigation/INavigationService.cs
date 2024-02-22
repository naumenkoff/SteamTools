using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamTools.Presentation.Services.Navigation;

public interface INavigationService
{
    ObservableObject CurrentView { get; }
    void Navigate<T>() where T : ObservableObject;
}