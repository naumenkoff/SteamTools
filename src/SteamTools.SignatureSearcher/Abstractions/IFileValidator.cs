namespace SteamTools.SignatureSearcher.Abstractions;

public interface IFileValidator<in T>
{
    bool Validate(T? value);
}