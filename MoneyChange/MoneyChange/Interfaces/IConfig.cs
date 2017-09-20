using SQLite.Net.Interop;

namespace MoneyChange.Interfaces
{
    public interface IConfig
    {
        string DirectoryDB { get; }
        ISQLitePlatform Platform { get; }
    }
}
