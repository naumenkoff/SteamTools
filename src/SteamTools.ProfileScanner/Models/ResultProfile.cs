using System.Text;
using SProject.Steam;
using SteamTools.Common;
using SteamTools.ProfileScanner.Enums;
using SteamTools.ProfileScanner.Models.ScanningResults;

namespace SteamTools.ProfileScanner.Models;

public sealed class ResultProfile(ISteamIDPair steamData) : ISteamIDPair
{
    private readonly Dictionary<ResultType, List<ResultBase>> _dictionary = new();
    private readonly object _lock = new();
    public int DetectionsCount { get; private set; }
    public SteamID32 ID32 { get; } = steamData.ID32;
    public SteamID64 ID64 { get; } = steamData.ID64;

    public string GetLogin()
    {
        lock (_lock)
        {
            var stringBuilder = new StringBuilder();
            foreach (var config in _dictionary[ResultType.Config]) stringBuilder.AppendLine(((ConfigResult)config).Login);
            foreach (var loginusers in _dictionary[ResultType.Loginusers])
                stringBuilder.AppendLine(((LoginusersResult)loginusers).Login);
            return stringBuilder.ToString();
        }
    }

    public void Attach(ResultBase resultBase)
    {
        if (SteamIDValidator.IsSteamID64(resultBase.ID64.AsLong) is false) return;
        lock (_lock)
        {
            DetectionsCount++;
            if (_dictionary.TryGetValue(resultBase.Type, out var localResults))
                localResults.Add(resultBase);
            else _dictionary[resultBase.Type] = [resultBase];
        }
    }
}