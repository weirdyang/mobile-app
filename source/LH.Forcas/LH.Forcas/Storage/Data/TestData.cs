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
                Name = "RB eKonto",
                AccountId = Guid.NewGuid(),
                AccountNumber = AccountNumber.FromCzLocal("123456798/5500"),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(5555758.17m, "CZK")
            });

            collection.Insert(new BankAccount
            {
                BankId = "AB",
                Name = "AB Spořící",
                AccountId = Guid.NewGuid(),
                AccountNumber = AccountNumber.FromCzLocal("555574789789/3300"),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(258.17m, "CZK")
            });

            collection.Insert(new CashAccount
            {
                Name = "Peněženka",
                AccountId = Guid.NewGuid(),
                CurrencyId = "CZK",
                CurrentBalance = new Amount(-38.17m, "CZK")
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