using SQLite.Net;
using SQLite.Net.Async;

namespace LH.Forcas.Storage
{
    public interface IDbManager
    {
        void Initialize();

        SQLiteConnection GetSyncConnection();

        SQLiteAsyncConnection GetAsyncConnection();
    }
}
