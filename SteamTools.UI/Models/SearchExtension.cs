namespace SteamTools.UI.Models;

public class SearchExtension
{
    public SearchExtension(string extension)
    {
        Extension = extension;
        Selected = false;
    }

    public string Extension { get; }
    public bool Selected { get; set; }
}