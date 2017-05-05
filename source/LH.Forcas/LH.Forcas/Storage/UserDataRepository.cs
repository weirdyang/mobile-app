﻿using System;
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
            return this.dbManager.LiteRepository
                .SingleOrDefault<UserSettings>(x => x.Id == UserSettings.SingleId);
        }

        public void SaveUserSettings(UserSettings settings)
        {
            this.dbManager.LiteRepository.Upsert(settings);
        }

        public IList<Account> GetAccounts()
        {
            return this.dbManager.LiteRepository.Fetch<Account>();
        }

        public void SaveAccount(Account account)
        {
            this.dbManager.LiteRepository.Upsert(account);
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
            this.dbManager.LiteRepository.DeleteAll<Account>();
            this.dbManager.LiteRepository.DeleteAll<Category>();
            this.dbManager.LiteRepository.DeleteAll<Budget>();
        }
#endif
    }
}