using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamTools.UI.ViewModels;

public class ProfileDataFetcherViewModel : ObservableObject
{
    private string _enteredText;

    public string EnteredText
    {
        get => _enteredText;
        set
        {
            _enteredText = value;
            MessageBox.Show(_enteredText);
            OnPropertyChanged();
        }
    }
}