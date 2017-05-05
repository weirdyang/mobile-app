using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Extensions;

namespace LH.Forcas.Storage
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly IDbManager dbManager;

        public UserDataRepository(IDbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public UserSettings GetUserSettings()
        {
            return this.dbManager.Database.SingleOrDefault<UserSettings>(UserSettings.BsonId);
        }

        public void SaveUserSettings(UserSettings settings)
        {
            this.dbManager.Database.Upsert(settings);
        }

        public IList<Account> GetAccounts()
        {
            return this.dbManager.Database.Fetch<Account>();
        }

        public void SaveAccount(Account account)
        {
            this.dbManager.Database.Upsert(account);
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
            this.dbManager.Database.DeleteAll<Account>();
            this.dbManager.Database.DeleteAll<Category>();
            this.dbManager.Database.DeleteAll<Budget>();
            this.dbManager.Database.DeleteAll<UserSettings>();
        }
#endif
    }
}