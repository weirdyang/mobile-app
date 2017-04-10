using System;
using LH.Forcas.Domain.UserData;

namespace LH.Forcas.Storage
{
    using Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using LiteDB;

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
                    .FindOne(x => x.UserSettingsId == UserSettings.SingleId);
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
    }
}