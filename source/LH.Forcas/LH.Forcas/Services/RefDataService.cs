using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.RefDataContract;
using LH.Forcas.Storage;
using LH.Forcas.Sync.RefData;

namespace LH.Forcas.Services
{
    using System.Collections.Concurrent;
    using System.Diagnostics;

    public class RefDataService : IRefDataService
    {
        private readonly IRefDataRepository repository;
        private readonly IRefDataDownloader downloader;
        private readonly IAnalyticsReporter analyticsReporter;

        private readonly object cacheLock = new object();
        private readonly IDictionary<Type, object> cache = new ConcurrentDictionary<Type, object>();

        public RefDataService(IRefDataRepository repository, IRefDataDownloader downloader, IAnalyticsReporter analyticsReporter)
        {
            this.repository = repository;
            this.downloader = downloader;
            this.analyticsReporter = analyticsReporter;
        }

        public IList<Bank> GetBanks()
        {
            return this.GetRefDataViaCache(() => this.repository.GetBanks());
        }

        public IList<Country> GetCountries()
        {
            return this.GetRefDataViaCache(() => this.repository.GetCountries());
        }

        public Country GetCountry(string id)
        {
            var countries = this.GetCountries();
            var unifiedId = id.ToUpper();

            return countries.Single(x => x.CountryId == unifiedId); // TODO: this should be done via dictionary...
        }

        public Currency GetCurrency(string id)
        {
            var currencies = this.GetCurrencies();

            return currencies.Single(x => x.CurrencyId == id); // TODO: this should be done via dictionary...
        }

        public IList<Currency> GetCurrencies()
        {
            return this.GetRefDataViaCache(() => this.repository.GetCurrencies());
        }

        public async Task UpdateRefData()
        {
            try
            {
                var currentStatus = this.repository.GetStatus();
                var result = await this.downloader.DownloadRefData(currentStatus);

                if (!result.NewDataAvailable)
                {
                    return;
                }

                this.repository.SaveRefDataUpdate(result.Data, result.NewStatus);
            }
            catch (Exception ex)
            {
                this.analyticsReporter.ReportHandledException(ex, "Updating RefData failed");
            }
        }

        private IList<TDomain> GetRefDataViaCache<TDomain>(Func<IEnumerable<TDomain>> fetchDataDelegate)
            where TDomain : IRefDataEntity
        {
            IList<TDomain> typedResult = null;

            try
            {
                lock (this.cacheLock)
                {
                    object result;
                    if (this.cache.TryGetValue(typeof(TDomain), out result))
                    {
                        return (IList<TDomain>)result;
                    }

                    var data = fetchDataDelegate.Invoke();
                    if (data != null)
                    {
                        typedResult = data.Where(x => x.IsActive).ToArray();
                    }

                    this.cache.Add(typeof(TDomain), typedResult);
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle network exceptions differently
                Debug.WriteLine(ex);
#if DEBUG
                throw;
#endif
            }

            return typedResult;
        }
    }
}