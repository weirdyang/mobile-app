using System;
using System.Collections.Generic;
using LH.Forcas.Domain.RefData;
using LH.Forcas.RefDataContract;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.Storage.Caching
{
    public class RefDataRepositoryCache : RepositoryCacheBase, IRefDataRepository
    {
        private RefDataStatus statusCacheStore;
        private IEnumerable<Bank> banksCacheStore;
        private IEnumerable<Country> countriesCacheStore;
        private IEnumerable<Currency> currenciesCacheStore;

        private readonly object sharedLock = new object();
        private readonly IRefDataRepository repository;

        public RefDataRepositoryCache(IRefDataRepository repository, IMvxMessenger messenger)
            : base(messenger)
        {
            this.repository = repository;
        }

        public RefDataStatus GetStatus()
        {
            return this.GetThroughCache(ref this.statusCacheStore, () => this.repository.GetStatus(), this.sharedLock);
        }

        public IEnumerable<Bank> GetBanks()
        {
            return this.GetThroughCache(ref this.banksCacheStore, () => this.repository.GetBanks(), this.sharedLock);
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return this.GetThroughCache(ref this.currenciesCacheStore, () => this.repository.GetCurrencies(), this.sharedLock);
        }

        public IEnumerable<Country> GetCountries()
        {
            return this.GetThroughCache(ref this.countriesCacheStore, () => this.repository.GetCountries(), this.sharedLock);
        }

        public void SaveRefDataUpdate(RefDataUpdate update, RefDataStatus status)
        {
            this.Invalidate(ref this.statusCacheStore, this.sharedLock);
            this.InvalidateIfUpdated(update.Banks, ref this.banksCacheStore);
            this.InvalidateIfUpdated(update.Countries, ref this.countriesCacheStore);
            this.InvalidateIfUpdated(update.Currencies, ref this.currenciesCacheStore);

            this.repository.SaveRefDataUpdate(update, status);
        }

        private void InvalidateIfUpdated<T>(ICollection<T> list, ref IEnumerable<T> cacheStore)
        {
            if (list != null && list.Count > 0)
            {
                this.Invalidate(ref cacheStore, this.sharedLock);
            }
        }

        protected override IEnumerable<Func<bool>> GetTrimPriorities()
        {
            yield return () => this.Invalidate(ref this.banksCacheStore, this.sharedLock);
            yield return () => this.Invalidate(ref this.countriesCacheStore, this.sharedLock);
            yield return () => this.Invalidate(ref this.currenciesCacheStore, this.sharedLock);
        }
    }
}