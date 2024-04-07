namespace SteamTools.Common;

public interface IFactory<out T>
{
    T Create();
}

public interface IFactory<in T1, out T2>
{
    T2 Create(T1 arg);
}