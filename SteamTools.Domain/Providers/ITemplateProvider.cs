using System.Text.RegularExpressions;

namespace SteamTools.Domain.Providers;

public interface ITemplateProvider<in T>
{
    Regex GetTemplate(T value);
}