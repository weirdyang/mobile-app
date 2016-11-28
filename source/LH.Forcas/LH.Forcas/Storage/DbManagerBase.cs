using System;
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
            connection.BeginTransaction();

            try
            {
                connection.CreateTable<CurrencyEntity>();
                connection.CreateTable<CountryEntity>();

                connection.Commit();
            }
            catch (Exception)
            {
                connection.Rollback();

                // TODO: Rethrow as custom exception
            }
        }

        public abstract SQLiteConnection GetSyncConnection();

        public abstract SQLiteAsyncConnection GetAsyncConnection();
    }
}
