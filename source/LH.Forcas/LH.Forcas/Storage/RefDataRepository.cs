using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Extensions;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Entities.RefData;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(RefDataRepository))]

namespace LH.Forcas.Storage
{
    public class RefDataRepository : IRefDataRepository
    {
        private readonly IDbManager dbManager;

        public RefDataRepository() : this(XamarinDependencyService.Default)
        {
        }

        public RefDataRepository(IDependencyService dependencyService)
        {
            this.dbManager = dependencyService.Get<IDbManager>();
        }

        public async Task<IList<T>> GetRefDataAsync<T>() where T : class, new()
        {
            return await this.dbManager.GetAsyncConnection()
                .Table<T>()
                .ToListAsync();
        }

        public async Task SaveRefDataUpdates(IRefDataUpdate[] updates)
        {
            // ReSharper disable once RedundantLambdaParameterType
            await this.dbManager.GetAsyncConnection().RunInTransactionAsync((SQLiteConnection conn) =>
            {
                var versions = conn.Table<RefDataVersionEntity>().ToList();

                foreach (var update in updates)
                {
                    var version = versions.SingleOrDefault(x => x.EntityTypeName == update.EntityType.Name);

                    if (version == null || version.Version < update.Version)
                    {
                        update.Data.ForEach(item => conn.InsertOrReplace(item));

                        if (version == null)
                        {
                            version = new RefDataVersionEntity();
                            version.EntityTypeName = update.EntityType.Name;
                        }

                        version.Version = update.Version;
                        conn.InsertOrReplace(version);
                    }
                }
            });
        }
    }
}