using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Extensions
{
    using System.Linq;
    using Domain.RefData;

    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action.Invoke(item);
            }
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> action)
        {
            foreach (var item in items)
            {
                await action.Invoke(item);
            }
        }

        public static Bank SingleById(this IEnumerable<Bank> banks, string bankId)
        {
            return banks.Single(x => x.BankId == bankId);
        }

        public static Country SingleById(this IEnumerable<Country> countries, string countryId)
        {
            return countries.Single(x => x.CountryId == countryId);
        }

        public static Currency SingleById(this IEnumerable<Currency> currencies, string currencyId)
        {
            return currencies.Single(x => x.CurrencyId == currencyId);
        }
    }
}