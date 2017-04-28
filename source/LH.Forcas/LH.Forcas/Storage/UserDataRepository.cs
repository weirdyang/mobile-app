using System;
using LH.Forcas.Domain.UserData;

namespace LH.Forcas.Storage
{
    using Extensions;
    using System.Collections.Generic;

    public class UserDataRepository : IUserDataRepository
    {
        private readonly IDbManager dbManager;

        public UserDataRepository(IDbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public UserSettings GetUserSettings()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<UserSettings>()
                    .FindOne(x => x.Id == UserSettings.SingleId);
            }
        }

        public void SaveUserSettings(UserSettings settings)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                db.GetCollection<UserSettings>().Upsert(settings);
            }
        }

        public IEnumerable<Account> GetAccounts()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<Account>().FindAll();
            }
        }

        public void SaveAccount(Account account)
        {
            using (var db = this.dbManager.GetDatabase())
            {
                db.GetCollection<Account>().Upsert(account);
            }
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
    }
}