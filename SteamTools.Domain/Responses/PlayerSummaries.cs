namespace SteamTools.Domain.Responses;

public record PlayerSummaries(string SteamID, int CommunityVisibilityState, int ProfileState, string PersonaName, string ProfileUrl, string Avatar,
    string AvatarMedium, string AvatarFull, string AvatarHash, int PersonaState, string RealName, string PrimaryClanID, int TimeCreated,
    int PersonaStateFlags, string LocCountryCode, string LocStateCode, int LocCityID);