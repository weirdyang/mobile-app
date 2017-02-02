using System;
using System.Collections.Generic;
using System.Linq;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Integration;
using LH.Forcas.Storage;

namespace LH.Forcas.Services
{
    using System.Collections.Concurrent;

    public class RefDataService : IRefDataService
    {
        private readonly ICrashReporter crashReporter;
        private readonly IRefDataRepository repository;

        private readonly object cacheLock = new object();
        private readonly IDictionary<Type, object> cache = new ConcurrentDictionary<Type, object>();

        public RefDataService(ICrashReporter crashReporter, IRefDataRepository repository)
        {
            this.crashReporter = crashReporter;
            this.repository = repository;
        }

        public IList<Bank> GetBanks()
        {
            return this.GetRefDataViaCache(() => this.repository.GetBanks());
        }

        public IList<Country> GetCountries()
        {
            return this.GetRefDataViaCache(() => this.repository.GetCountries());
        }

        public Currency GetCurrency(string id)
        {
            var currencies = this.GetRefDataViaCache(() => this.repository.GetCurrencies());

            return currencies.Single(x => x.CurrencyId == id); // TODO: this should be done via dictionary...
        }

        public IList<Currency> GetCurrencies()
        {
            return this.GetRefDataViaCache(() => this.repository.GetCurrencies());
        }

        private IList<TDomain> GetRefDataViaCache<TDomain>(Func<IEnumerable<TDomain>> fetchDataDelegate)
            where TDomain : IIsActive
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
                this.crashReporter.ReportFatal(ex);
                throw;
            }

            return typedResult;
        }
    }
}