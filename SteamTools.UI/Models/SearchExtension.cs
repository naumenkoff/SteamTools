using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SteamTools.UI.Models;

public class SearchExtension : ObservableObject
{
    private bool _selected;

    public SearchExtension(string extension)
    {
        Extension = extension;
    }

    public string Extension { get; }

    public bool Selected
    {
        get => _selected;
        set
        {
            if (_selected == value) return;

            _selected = value;
            OnPropertyChanged();
        }
    }

    public override bool Equals(object obj)
    {
        return obj is SearchExtension searchExtension && Equals(searchExtension);
    }

    protected bool Equals(SearchExtension other)
    {
        return Extension.Equals(other.Extension, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Extension);
    }
}