using LH.Forcas.Contract;
using LH.Forcas.Storage.Entities.RefData;
using SQLite;

namespace LH.Forcas.Storage
{
    public abstract class DbManagerBase : IDbManager
    {
        protected const string DbFileName = "Forcas.db3";

        public void Initialize()
        {
            var connection = this.GetSyncConnection();

            connection.CreateTable<CurrencyEntity>();
        }

        public abstract SQLiteConnection GetSyncConnection();

        public abstract SQLiteAsyncConnection GetAsyncConnection();
    }
}
