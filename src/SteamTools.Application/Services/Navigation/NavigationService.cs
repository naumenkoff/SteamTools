﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamTools.Presentation.Services.Navigation;

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ObservableObject> _viewModelFactory;
    private ObservableObject _currentView;

    public NavigationService(Func<Type, ObservableObject> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public ObservableObject CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void Navigate<T>() where T : ObservableObject
    {
        CurrentView = _viewModelFactory.Invoke(typeof(T));
    }
}