using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Integration;
using LH.Forcas.Storage;

namespace LH.Forcas.Services
{
    public class RefDataService : IRefDataService
    {
        private readonly ICrashReporter crashReporter;
        private readonly IRefDataDownloader downloader;
        private readonly IRefDataRepository repository;

        private readonly SemaphoreSlim cacheSemaphore = new SemaphoreSlim(1, 1);
        private readonly IDictionary<Type, object> cache = new Dictionary<Type, object>();

        public RefDataService(ICrashReporter crashReporter, IRefDataDownloader downloader, IRefDataRepository repository)
        {
            this.crashReporter = crashReporter;
            this.downloader = downloader;
            this.repository = repository;
        }

        public async Task<IList<Bank>> GetBanks()
        {
            return await this.GetRefDataViaCache(() => this.repository.GetBanks());
        }

        public async Task<IList<Country>> GetCountriesAsync()
        {
            return await this.GetRefDataViaCache(() => this.repository.GetCountries());
        }

        public async Task<IList<Currency>> GetCurrencies()
        {
            return await this.GetRefDataViaCache(() => this.repository.GetCurrencies());
        }

        public async Task UpdateRefDataAsync()
        {
            try
            {
                var lastSyncTime = DateTime.MaxValue; // TODO: !!!

                IRefDataUpdate[] updates;
                try
                {
                     updates = await this.downloader.GetRefDataUpdates(lastSyncTime);
                }
                catch (Exception ex)
                {
                    this.crashReporter.ReportException(ex);
                    return;
                }

                if (updates == null || !updates.Any())
                {
                    return; // No updates available
                }

                this.repository.SaveRefDataUpdates(updates);

                this.cache.Clear(); // Invalidate cache
            }
            catch (Exception ex)
            {
                this.crashReporter.ReportException(ex);
                throw;
            }
        }

        private async Task<IList<TDomain>> GetRefDataViaCache<TDomain>(Func<IEnumerable<TDomain>> fetchDataDelegate)
        {
            await this.cacheSemaphore.WaitAsync();

            object result;
            if (this.cache.TryGetValue(typeof(TDomain), out result))
            {
                return (IList<TDomain>)result;
            }

            IList<TDomain> typedResult = null;

            try
            {
                var data = fetchDataDelegate.Invoke();
                if (data != null)
                {
                    typedResult = data.ToArray();
                }
            }
            catch (Exception ex)
            {
                this.cacheSemaphore.Release();
                this.crashReporter.ReportFatal(ex);
                throw;
            }

            this.cache.Add(typeof(TDomain), typedResult);
            this.cacheSemaphore.Release();

            return typedResult;
        }
    }
}