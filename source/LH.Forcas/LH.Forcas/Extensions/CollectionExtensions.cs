using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Extensions
{
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
    }
}