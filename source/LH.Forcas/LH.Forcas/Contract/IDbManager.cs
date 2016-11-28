using SQLite;

namespace LH.Forcas.Contract
{
    public interface IDbManager
    {
        void Initialize();

        SQLiteConnection GetSyncConnection();

        SQLiteAsyncConnection GetAsyncConnection();
    }
}
