using System.Text.RegularExpressions;
using SteamTools.Domain.Enumerations;

namespace SteamTools.Domain.Providers;

public interface ITemplateProvider<in T>
{
    Regex GetTemplate(T value);
}