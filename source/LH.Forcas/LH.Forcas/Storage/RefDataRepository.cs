using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Extensions;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Entities.RefData;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(RefDataRepository))]

namespace LH.Forcas.Storage
{
    public class RefDataRepository : IRefDataRepository
    {
        private static readonly IDictionary<Type, Type> DomainToEntityMapping = new Dictionary<Type, Type>
        {
            { typeof(Country), typeof(CountryEntity) },
            { typeof(Bank), typeof(BankEntity) },
            { typeof(Currency), typeof(CurrencyEntity) },
        };

        private readonly IDbManager dbManager;

        public RefDataRepository(IDbManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public async Task<IList<Bank>> GetBanksAsync()
        {
            var result = await this.dbManager.GetAsyncConnection()
                .Table<BankEntity>()
                .ToListAsync();

            return Mapper.Instance.Map<List<Bank>>(result);
        }

        public async Task<IList<Currency>> GetCurrenciesAsync()
        {
            var result = await this.dbManager.GetAsyncConnection()
                .Table<CurrencyEntity>()
                .ToListAsync();

            return Mapper.Instance.Map<List<Currency>>(result);
        }

        public async Task<IList<Country>> GetCountriesAsync()
        {
            var result = await this.dbManager.GetAsyncConnection()
               .Table<CountryEntity>()
               .ToListAsync();

            return Mapper.Instance.Map<List<Country>>(result);
        }

        public async Task SaveRefDataUpdates(IRefDataUpdate[] updates)
        {
            // ReSharper disable once RedundantLambdaParameterType
            await this.dbManager.GetAsyncConnection().RunInTransactionAsync((SQLiteConnection conn) =>
            {
                var currentVersions = conn.Table<RefDataVersionEntity>().ToList();

                foreach (var update in updates)
                {
                    var version = currentVersions.SingleOrDefault(x => x.EntityTypeName == update.DomainType.Name);

                    if (version == null || version.Version < update.Version)
                    {
                        var targetType = DomainToEntityMapping[update.DomainType];
                        
                        update.Data.ForEach(item =>
                        {
                            var mapped = Mapper.Map(item, update.DomainType, targetType);
                            conn.InsertOrReplace(mapped);
                        });

                        if (version == null)
                        {
                            version = new RefDataVersionEntity();
                            version.EntityTypeName = update.DomainType.Name;
                        }

                        version.Version = update.Version;
                        conn.InsertOrReplace(version);
                    }
                }
            });
        }
    }
}