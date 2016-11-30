using SQLite;

namespace LH.Forcas.Storage
{
    public interface IDbManager
    {
        void Initialize();

        SQLiteConnection GetSyncConnection();

        SQLiteAsyncConnection GetAsyncConnection();
    }
}
