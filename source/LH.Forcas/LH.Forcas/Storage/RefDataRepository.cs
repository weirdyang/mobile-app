using System.Collections.Generic;
using System.Linq;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Extensions;

namespace LH.Forcas.Storage
{
    public class RefDataRepository : IRefDataRepository
    {
        private readonly IDbManager dbManager;

        public RefDataRepository(IDbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public IEnumerable<Bank> GetBanks()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<Bank>().FindAll();
            }
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<Currency>().FindAll();
            }
        }

        public IEnumerable<Country> GetCountries()
        {
            using (var db = this.dbManager.GetDatabase())
            {
                return db.GetCollection<Country>().FindAll();
            }
        }

        public void SaveRefDataUpdates(IRefDataUpdate[] updates)
        {
            using (var db = this.dbManager.GetDatabase())
            using (var tx = db.BeginTrans())
            {
                var currentVersions = db.GetCollection<RefDataVersion>()
                    .FindAll()
                    .ToArray();

                foreach (var update in updates)
                {
                    var currentVersion = currentVersions.SingleOrDefault(x => x.TypeName == update.TypeName);
                    var shouldUpdate = currentVersion == null || currentVersion.Version < update.Version;
                    var shouldInsertRefDataVersion = false;

                    if (currentVersion == null)
                    {
                        currentVersion = new RefDataVersion();
                        currentVersion.TypeName = update.TypeName;

                        shouldInsertRefDataVersion = true;
                    }

                    if (shouldUpdate)
                    {
                        var collection = db.GetCollection(update.TypeName);
                        
                        update.Data.ForEach(item =>
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            var bson = db.Mapper.ToDocument(update.Type, item);
                            collection.Upsert(bson);
                        });

                        currentVersion.Version = update.Version;
                        db.GetCollection<RefDataVersion>().Upsert(currentVersion, shouldInsertRefDataVersion);
                    }
                }

                tx.Commit();
            }
        }
    }
}