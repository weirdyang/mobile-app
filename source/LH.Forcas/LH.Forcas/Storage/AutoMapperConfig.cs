using System.Collections.Generic;
using AutoMapper;
using LH.Forcas.Domain.UserData;
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

            config.CreateMap<List<BudgetCategory>, string>().ConvertUsing(JsonConvert.SerializeObject);
            config.CreateMap<string, List<BudgetCategory>>().ConvertUsing(DeserializeJson<List<BudgetCategory>>);

            // Ref Data

            // User Data
            config.CreateMap<TransactionEntity, Transaction>().ReverseMap();

            config.CreateMap<BudgetEntity, Budget>()
                .ForMember(x => x.Categories, options => options.MapFrom(entity => entity.CategoriesJson))
                .ForMember(x => x.Month, options => options.ResolveUsing(entity => entity.BudgetId % 100))
                .ForMember(x => x.Year, options => options.ResolveUsing(entity => entity.BudgetId / 100));

            config.CreateMap<Budget, BudgetEntity>()
                .ForMember(x => x.BudgetId, options => options.ResolveUsing(domain => domain.Year*100 + domain.Month))
                .ForMember(x => x.CategoriesJson, options => options.MapFrom(domain => domain.Categories));
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