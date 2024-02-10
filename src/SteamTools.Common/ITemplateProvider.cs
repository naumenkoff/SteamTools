using System.Text.RegularExpressions;

namespace SteamTools.Common;

public interface ITemplateProvider<in T>
{
    Regex GetTemplate(T value);
}