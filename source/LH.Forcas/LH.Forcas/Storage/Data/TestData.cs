namespace LH.Forcas.Storage.Data
{
    using System;
    using System.Collections.Generic;
    using Domain.UserData;
    using Extensions;
    using LiteDB;

    public static class TestData
    {
        public static void InsertTestData(IDbManager dbManager)
        {
            using (var db = dbManager.GetDatabase())
            {
                db.DropCollection(nameof(Category));
                db.DropCollection(nameof(Account));
                db.DropCollection(nameof(Budget));

                InsertAccounts(db);
                InsertCategories(db);
            }
        }

        private static void InsertAccounts(LiteDatabase db)
        {
            var collection = db.GetCollection<Account>();

            collection.Insert(new BankAccount
            {
                BankId = "RB",
                Type = BankAccountType.Checking,
                Name = "RB eKonto",
                AccountId = Guid.NewGuid(),
                AccountNumber = AccountNumber.Parse("123456798/5500"),
                CurrencyId = "CZK"
            });

            collection.Insert(new BankAccount
            {
                BankId = "AB",
                Type = BankAccountType.Savings,
                Name = "AB Spořící",
                AccountId = Guid.NewGuid(),
                AccountNumber = AccountNumber.Parse("555574789789/3300"),
                CurrencyId = "CZK"
            });
        }

        private static void InsertCategories(LiteDatabase db)
        {
            var collection = db.GetCollection<Category>();

            collection.Insert(new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = "Nákupy",
                Children = new List<Category>
                                  {
                                      new Category
                                      {
                                          CategoryId = Guid.NewGuid(),
                                          Name = "Oblečení"
                                      },
                                      new Category
                                      {
                                          CategoryId = Guid.NewGuid(),
                                          Name = "Potraviny"
                                      }
                                  }
            });

            collection.Insert(new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = "Auto",
                Children = new List<Category>
                                  {
                                      new Category
                                      {
                                          CategoryId = Guid.NewGuid(),
                                          Name = "Servis"
                                      },
                                      new Category
                                      {
                                          CategoryId = Guid.NewGuid(),
                                          Name = "Benzín"
                                      }
                                  }
            });
        }
    }
}