using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LH.Forcas.Banking.Exceptions;
using LH.Forcas.Domain.UserData.Authorization;
using Microsoft.Practices.Unity;

namespace LH.Forcas.Banking.Providers
{
    public class BankProviderFactory : IBankProviderFactory
    {
        private readonly IList<string> supportedBankIds;
        private readonly IUnityContainer container;

        public BankProviderFactory(IUnityContainer container)
        {
            this.container = container;
            this.supportedBankIds = new List<string>();
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
                    this.container.RegisterType(typeof(BankAuthorizationBase), attribute.AuthorizationType, bankId, new TransientLifetimeManager());
                    this.container.RegisterType(typeof(IBankProvider), providerType, bankId, new TransientLifetimeManager());

                    this.supportedBankIds.Add(bankId);
                }
            }
        }

        public BankAuthorizationBase CreateAuthorization(string bankId)
        {
            if (!this.supportedBankIds.Contains(bankId))
            {
                throw new BankNotSupportedException(bankId);
            }

            return this.container.Resolve<BankAuthorizationBase>(bankId);
        }

        public IBankProvider CreateProvider(string bankId)
        {
            if (!this.supportedBankIds.Contains(bankId))
            {
                throw new BankNotSupportedException(bankId);
            }

            return this.container.Resolve<IBankProvider>(bankId);
        }
    }
}