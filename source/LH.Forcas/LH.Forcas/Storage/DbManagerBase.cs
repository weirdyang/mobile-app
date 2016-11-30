using LH.Forcas.Domain.RefData;
using LH.Forcas.Storage.Entities.RefData;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

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
                connection.CreateTable<RefDataVersionEntity>();
            }
        }

        public SQLiteConnection GetSyncConnection()
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            return new SQLiteConnection(this.GetPlatform(), this.GetDbFilePath(), true);
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(this.GetSyncConnectionWithLock);
        }

        protected SQLiteConnectionWithLock GetSyncConnectionWithLock()
        {
            return new SQLiteConnectionWithLock(this.GetPlatform(), this.GetConnectionString());
        }

        protected SQLiteConnectionString GetConnectionString()
        {
            return new SQLiteConnectionString(this.GetDbFilePath(), true);
        }

        protected abstract ISQLitePlatform GetPlatform();

        protected abstract string GetDbFilePath();
    }
}
