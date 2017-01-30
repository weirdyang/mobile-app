namespace LH.Forcas.Integration.Banks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public class BankProviderCatalog : IBankProviderCatalog
    {
        private readonly IUnityContainer container;
        private readonly IDictionary<string, Tuple<Type, Type>> providers;

        public BankProviderCatalog(IUnityContainer container)
        {
            this.providers = new Dictionary<string, Tuple<Type, Type>>();
            this.container = container;
        }

        public void Initialize(IEnumerable<Type> providerTypes = null)
        {
            if (providerTypes == null)
            {
                var providerInterfaceInfo = typeof(IBankProvider).GetTypeInfo();

                var assembly = this.GetType().GetTypeInfo().Assembly;

                providerTypes = assembly.DefinedTypes
                    .Where(x => providerInterfaceInfo.IsAssignableFrom(x))
                    .Select(x => x.AsType());
            }

            foreach (var providerType in providerTypes)
            {
                var attribute = providerType.GetTypeInfo().GetCustomAttribute<BankProviderInfoAttribute>();
                var typesTuple = new Tuple<Type, Type>(providerType, attribute.AuthorizationType);

                foreach (var bankId in attribute.BankIds)
                {
                    // TODO: Log this
                    this.providers.Add(bankId, typesTuple);
                }
            }
        }

        public Type GetAuthorizationType(string bankId)
        {
            var providerTypes = this.GetProviderTypes(bankId);

            return providerTypes.Item2;
        }

        public IBankProvider GetIntegrationProvider(string bankId)
        {
            var providerTypes = this.GetProviderTypes(bankId);

            return (IBankProvider)this.container.Resolve(providerTypes.Item1);
        }

        private Tuple<Type, Type> GetProviderTypes(string bankId)
        {
            Tuple<Type, Type> result;
            if (!this.providers.TryGetValue(bankId, out result))
            {
                throw new ArgumentException($"Provider for the BankId {bankId} could not be found in the catalog.", nameof(bankId));
            }

            return result;
        }
    }
}