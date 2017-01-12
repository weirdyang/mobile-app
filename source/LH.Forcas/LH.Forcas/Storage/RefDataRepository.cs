namespace LH.Forcas.Storage
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Domain.RefData;
    using Newtonsoft.Json;

    public class RefDataRepository : IRefDataRepository
    {
        public IEnumerable<Bank> GetBanks()
        {
            return this.ReadJsonResource<Bank>();
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return this.ReadJsonResource<Currency>();
        }

        public IEnumerable<Country> GetCountries()
        {
            return this.ReadJsonResource<Country>();
        }

        private IEnumerable<T> ReadJsonResource<T>()
        {
            var type = this.GetType();
            var resourceName = $"{type.Namespace}.Data.{typeof(T).Name}.json";

            var assembly = type.GetTypeInfo().Assembly;

            foreach (var name in assembly.GetManifestResourceNames())
            {
                Debug.WriteLine(name);
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"The resource with the name {resourceName} could not be found.");
                }

                using (var textReader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(textReader))
                {
                    var serializer = new JsonSerializer();
                    var result = serializer.Deserialize(jsonReader, typeof(T).MakeArrayType());

                    return (IEnumerable<T>) result;
                }
            }
        }
    }
}