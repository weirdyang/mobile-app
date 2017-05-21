using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.Storage.Caching
{
    public class UserDataRepositoryCache : RepositoryCacheBase, IUserDataRepository
    {
        private UserSettings userSettingsCacheStore;
        private IList<Account> accountsCacheStore;

        private readonly IUserDataRepository repository;
        private readonly object userSettingsLock = new object();
        private readonly object accountsLock = new object();

        public UserDataRepositoryCache(IUserDataRepository repository, IMvxMessenger messenger)
            : base(messenger)
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

        public IList<Account> GetAccounts()
        {
            return this.GetThroughCache(
                ref this.accountsCacheStore, 
                () => this.repository.GetAccounts(), 
                this.accountsLock);
        }

        public void SaveAccount(Account account)
        {
            this.Invalidate(ref this.accountsCacheStore, this.accountsLock);
            this.repository.SaveAccount(account);
        }

        public void SoftDeleteAccount(Guid id)
        {
            this.Invalidate(ref this.accountsCacheStore, this.accountsLock);
            this.repository.SoftDeleteAccount(id);
        }

        public IList<Category> GetCategories()
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

#if DEBUG
        public void DeleteAll()
        {
            this.repository.DeleteAll();
        }
#endif

        protected override IEnumerable<Func<bool>> GetTrimPriorities()
        {
            yield return () => this.Invalidate(ref this.userSettingsCacheStore, this.userSettingsLock);
        }
    }
}