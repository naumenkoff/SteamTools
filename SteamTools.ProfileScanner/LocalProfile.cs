using System.Collections.Concurrent;
using System.Text;
using SProject.Steam;
using SteamTools.Common;

namespace SteamTools.ProfileScanner;

public class LocalProfile : ISteamIDPair
{
    private readonly ConcurrentDictionary<LocalResultType, List<LocalResult>> _dictionary;

    public LocalProfile(ISteamIDPair steamData)
    {
        _dictionary = new ConcurrentDictionary<LocalResultType, List<LocalResult>>();
        ID32 = steamData.ID32;
        ID64 = steamData.ID64;
    }

    public int DetectionsCount { get; private set; }

    public SteamID32 ID32 { get; }
    public SteamID64 ID64 { get; }

    public string GetLogin()
    {
        var stringBuilder = new StringBuilder();

        foreach (var config in _dictionary[LocalResultType.Config]) stringBuilder.AppendLine(((ConfigData)config).Login);

        foreach (var loginusers in _dictionary[LocalResultType.Loginusers]) stringBuilder.AppendLine(((LoginusersData)loginusers).Login);

        return stringBuilder.ToString();
    }

    public void Attach(LocalResult localResult)
    {
        if (SteamIDValidator.IsSteamID64(localResult.ID64.AsLong) is false) return;

        _dictionary.AddOrUpdate(localResult.Type, _ => [localResult], (_, list) =>
        {
            list.Add(localResult);
            return list;
        });

        DetectionsCount++;
    }
}