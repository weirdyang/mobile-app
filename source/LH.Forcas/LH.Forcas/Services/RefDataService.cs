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
    using System.Diagnostics.Contracts;

    public class RefDataService : IRefDataService
    {
        private readonly ICrashReporter crashReporter;
        private readonly IRefDataRepository repository;

        private readonly SemaphoreSlim cacheSemaphore = new SemaphoreSlim(1, 1);
        private readonly IDictionary<Type, object> cache = new Dictionary<Type, object>();

        public RefDataService(ICrashReporter crashReporter, IRefDataRepository repository)
        {
            this.crashReporter = crashReporter;
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

        public async Task<IList<Bank>> GetBanksByCountry(string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new ArgumentNullException(nameof(countryCode));
            }

            var allBanks = await this.GetBanks();
            return allBanks
                .Where(x => string.Equals(x.CountryCode, countryCode, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        private async Task<IList<TDomain>> GetRefDataViaCache<TDomain>(Func<IEnumerable<TDomain>> fetchDataDelegate)
            where TDomain : IIsActive
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
                    typedResult = data.Where(x => x.IsActive).ToArray();
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