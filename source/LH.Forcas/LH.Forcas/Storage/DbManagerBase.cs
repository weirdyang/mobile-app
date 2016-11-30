using LH.Forcas.Domain.RefData;
using SQLite;

namespace LH.Forcas.Storage
{
    public abstract class DbManagerBase : IDbManager
    {
        protected const string DbFileName = "Forcas.db3";

        public void Initialize()
        {
            using (var connection = this.GetSyncConnection())
            {
                connection.CreateTable<Currency>();
                connection.CreateTable<Country>();
                connection.CreateTable<Bank>();
                connection.CreateTable<RefDataVersion>();
            }
        }

        public abstract SQLiteConnection GetSyncConnection();

        public abstract SQLiteAsyncConnection GetAsyncConnection();
    }
}
