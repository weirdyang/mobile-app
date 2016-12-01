using AutoMapper;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Storage.Entities.RefData;
using LH.Forcas.Storage.Entities.UserData;
using Newtonsoft.Json;

namespace LH.Forcas.Storage
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(DefineMaps);
            Mapper.AssertConfigurationIsValid();
        }

        private static void DefineMaps(IMapperConfigurationExpression config)
        {
            // General types
            config.CreateMap<int, TransactionType>().ConvertUsing(x => (TransactionType)x);
            config.CreateMap<TransactionType, int>().ConvertUsing(x => (int)x);

            ConfigureRefDataMapping(config);

            // User Data
            //config.CreateMap<TransactionEntity, Transaction>().ReverseMap();

            //config.CreateMap<BudgetEntity, Budget>()
            //    .ForMember(x => x.Month, options => options.ResolveUsing(entity => entity.BudgetId % 100))
            //    .ForMember(x => x.Year, options => options.ResolveUsing(entity => entity.BudgetId / 100));

            //config.CreateMap<Budget, BudgetEntity>()
            //    .ForMember(x => x.BudgetId, options => options.ResolveUsing(domain => domain.Year*100 + domain.Month));

            //config.CreateMap<BudgetCategory, BudgetCategoryEntity>().ReverseMap();
        }

        private static void ConfigureRefDataMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<short, PrefferedCcySymbolLocation>().ConvertUsing(x => (PrefferedCcySymbolLocation)x);
            config.CreateMap<PrefferedCcySymbolLocation, short>().ConvertUsing(x => (short)x);

            config.CreateMap<BankEntity, Bank>();
            config.CreateMap<Bank, BankEntity>()
                .ForMember(x => x.Country, opt => opt.Ignore());

            config.CreateMap<CountryEntity, Country>();
            config.CreateMap<Country, CountryEntity>()
                .ForMember(x => x.DefaultCurrency, opt => opt.Ignore());

            config.CreateMap<CurrencyEntity, Currency>();
            config.CreateMap<Currency, CurrencyEntity>();
        }

        private static T DeserializeJson<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}