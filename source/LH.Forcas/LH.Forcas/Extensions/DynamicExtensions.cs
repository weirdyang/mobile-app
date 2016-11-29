using System.Collections.Generic;
using System.Dynamic;

namespace LH.Forcas.Extensions
{
    public static class DynamicExtensions
    {
        public static bool TryGetPropertyValue<T>(this ExpandoObject expando, string propertyName, out T value)
        {
            var dict = (IDictionary<string, object>) expando;

            object result;
            if (dict.TryGetValue(propertyName, out result))
            {
                value = (T) result;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}