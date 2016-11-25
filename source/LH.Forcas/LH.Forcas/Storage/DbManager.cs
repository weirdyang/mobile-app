using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Extensions;
using LH.Forcas.Models;
using LH.Forcas.Models.RefData;
using SQLite;

namespace LH.Forcas.Storage
{
    public class DbManager : IDbManager
    {
        protected const string DbFileName = "Forcas.db3";

        public async Task Initialize()
        {
            await this.GetConnection().CreateTableIfNotExistsAsync<Bank>();
            await this.GetConnection().CreateTableIfNotExistsAsync<Currency>();
        }

        public SQLiteAsyncConnection GetConnection()
        {
            var path = PCLStorage.PortablePath.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, DbFileName);
            return new SQLiteAsyncConnection(path);
        }
    }
}
