using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Storage;
using Xamarin.Forms;

[assembly:Dependency(typeof(RefDataRepository))]

namespace LH.Forcas.Storage
{
    public class RefDataRepository : IRefDataRepository
    {
        private readonly IDbManager dbManager;
        private readonly IRefDataDownloader refDataDownloader;

        public RefDataRepository() : this(XamarinDependencyService.Default)
        {
        }

        public RefDataRepository(IDependencyService dependencyService)
        {
            this.refDataDownloader = dependencyService.Get<IRefDataDownloader>();
            this.dbManager = dependencyService.Get<IDbManager>();
        }

        public async Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new()
        {
            return await this.dbManager.GetAsyncConnection().Table<T>().ToListAsync();
        }

        public Task UpdateAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}