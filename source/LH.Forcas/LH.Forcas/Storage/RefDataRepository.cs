using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Extensions;
using LH.Forcas.Models.RefData;
using LH.Forcas.Storage;
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

        public async Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new()
        {
            return await this.dbManager.GetAsyncConnection()
                .Table<T>()
                .ToListAsync();
        }

        public async Task SaveRefDataUpdates(RefDataUpdateBase[] updated)
        {
            // TODO: Version info - load & save, create an entity for this

            await this.dbManager.GetAsyncConnection().RunInTransactionAsync(connection =>
            {
                updated.ForEach(item => connection.InsertOrReplace(item));
            });
        }
    }
}