using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData;

namespace LH.Forcas.Storage
{
    public interface IUserDataRepository
    {
        UserSettings GetUserSettings();

        void SaveUserSettings(UserSettings settings);

        IEnumerable<Account> GetAccounts();

        Account GetAccount(Guid id);

        void SaveAccount(Account account);

        IEnumerable<Category> GetCategories();

        Category GetCategory(Guid id);

        void SaveCategory(Category category);
    }
}