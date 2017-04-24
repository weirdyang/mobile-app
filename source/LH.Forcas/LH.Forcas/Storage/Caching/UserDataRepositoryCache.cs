using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData;
using Microsoft.Practices.Unity;
using Prism.Events;

namespace LH.Forcas.Storage.Caching
{
    public class UserDataRepositoryCache : RepositoryCacheBase, IUserDataRepository
    {
        private UserSettings userSettingsCacheStore;

        private readonly IUserDataRepository repository;
        private readonly object userSettingsLock = new object();

        public UserDataRepositoryCache([Dependency("Repository")] IUserDataRepository repository, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.repository = repository;
        }

        public UserSettings GetUserSettings()
        {
            return this.GetThroughCache(
                ref this.userSettingsCacheStore,
                () => this.repository.GetUserSettings(),
                this.userSettingsLock);
        }

        public void SaveUserSettings(UserSettings settings)
        {
            this.Invalidate(ref this.userSettingsCacheStore, this.userSettingsLock);
            this.repository.SaveUserSettings(settings);
        }

        public IEnumerable<Account> GetAccounts()
        {
            throw new NotImplementedException();
        }

        public Account GetAccount(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SaveAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Category GetCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public void SaveCategory(Category category)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Func<bool>> GetTrimPriorities()
        {
            yield return () => this.Invalidate(ref this.userSettingsCacheStore, this.userSettingsLock);
        }
    }
}