using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace SteamTools.UI.Models;

public readonly struct ProfileContent
{
    private const string UnknownValue = "Unknown";

    public ProfileContent(string header, [AllowNull] string value, ICommand command)
    {
        Header = header;
        Value = value ?? UnknownValue;
        Command = command;
    }

    public string Header { get; }

    public string Value { get; }

    public ICommand Command { get; }
}