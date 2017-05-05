using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData;

namespace LH.Forcas.Storage
{
    public interface IUserDataRepository
    {
        UserSettings GetUserSettings();

        void SaveUserSettings(UserSettings settings);

        IList<Account> GetAccounts();

        void SaveAccount(Account account);

        IList<Category> GetCategories();

        Category GetCategory(Guid id);

        void SaveCategory(Category category);

#if DEBUG
        void DeleteAll();
#endif
    }
}