using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LH.Forcas.Banking.Exceptions;
using LH.Forcas.Domain.UserData.Authorization;
using MvvmCross.Platform.IoC;

namespace LH.Forcas.Banking.Providers
{
    public class BankProviderFactory : IBankProviderFactory
    {
        private readonly IMvxIoCProvider iocProvider;
        private readonly IDictionary<string, BankMapping> mappings;

        public BankProviderFactory(IMvxIoCProvider iocProvider)
        {
            this.iocProvider = iocProvider;
            this.mappings = new ConcurrentDictionary<string, BankMapping>();
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

                foreach (var bankId in attribute.BankIds)
                {
                    var mapping = new BankMapping
                    {
                        AuthorizationType = attribute.AuthorizationType,
                        ProviderType = providerType
                    };

                    this.mappings.Add(bankId, mapping);
                }
            }
        }

        public BankAuthorizationBase CreateAuthorization(string bankId)
        {
            var mapping = this.GetMapping(bankId);

            return (BankAuthorizationBase)this.iocProvider.IoCConstruct(mapping.AuthorizationType);
        }

        public IBankProvider CreateProvider(string bankId)
        {
            var mapping = this.GetMapping(bankId);

            return (IBankProvider)this.iocProvider.IoCConstruct(mapping.ProviderType);
        }

        private BankMapping GetMapping(string bankId)
        {
            if (!this.mappings.TryGetValue(bankId, out BankMapping mapping))
            {
                throw new BankNotSupportedException(bankId);
            }

            return mapping;
        }

        private class BankMapping
        {
            public Type AuthorizationType { get; set; }

            public Type ProviderType { get; set; }
        }
    }
}