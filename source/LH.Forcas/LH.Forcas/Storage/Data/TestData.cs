using System.Diagnostics.CodeAnalysis;

namespace LH.Forcas.Storage.Data
{
    using System;
    using System.Collections.Generic;
    using Domain.UserData;

    [ExcludeFromCodeCoverage]
    public static class TestData
    {
        public static void InsertTestData(IUserDataRepository userDataRepository)
        {
            userDataRepository.DeleteAll();

            InsertAccounts(userDataRepository);
            // InsertCategories(userDataRepository);
        }

        private static void InsertAccounts(IUserDataRepository userDataRepository)
        {
            userDataRepository.SaveAccount(new CheckingAccount
            {
                BankId = "RB",
                Name = "RB eKonto",
                Id = Guid.NewGuid(),
                AccountNumber = AccountNumber.FromCzLocal("123456798/5500"),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(5555758.17m, "CZK"),
                LastSyncUtcTime = DateTime.UtcNow.AddDays(-2)
            });

            userDataRepository.SaveAccount(new SavingsAccount
            {
                BankId = "AB",
                Name = "AB Spořící",
                Id = Guid.NewGuid(),
                AccountNumber = AccountNumber.FromCzLocal("555574789789/3300"),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(258.17m, "CZK"),
                LastSyncUtcTime = DateTime.UtcNow.AddHours(-5)
            });

            userDataRepository.SaveAccount(new CashAccount
            {
                Name = "Peněženka",
                Id = Guid.NewGuid(),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(-38.17m, "CZK"),
                LastSyncUtcTime = DateTime.UtcNow.AddMinutes(-25)
            });
        }

        private static void InsertCategories(IUserDataRepository userDataRepository)
        {
            userDataRepository.SaveCategory(new Category
            {
                Id = Guid.NewGuid(),
                Name = "Nákupy",
                Icon = "md-shopping-cart",
                Children = new List<Category>
                                  {
                                      new Category
                                      {
                                          Id = Guid.NewGuid(),
                                          Name = "Oblečení"
                                      },
                                      new Category
                                      {
                                          Id = Guid.NewGuid(),
                                          Name = "Potraviny"
                                      }
                                  }
            });

            userDataRepository.SaveCategory(new Category
            {
                Id = Guid.NewGuid(),
                Name = "Auto",
                Icon = "md-directions-car",
                Children = new List<Category>
                                  {
                                      new Category
                                      {
                                          Id = Guid.NewGuid(),
                                          Name = "Servis"
                                      },
                                      new Category
                                      {
                                          Id = Guid.NewGuid(),
                                          Name = "Benzín"
                                      }
                                  }
            });
        }
    }
}