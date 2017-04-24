using System;
using System.Collections.Generic;
using LH.Forcas.Domain.UserData.Authorization;

namespace LH.Forcas.Banking.Providers
{
    public interface IBankProviderFactory
    {
        void Initialize(IEnumerable<Type> providerTypes = null);

        BankAuthorizationBase CreateAuthorization(string bankId);

        IBankProvider CreateProvider(string bankId);
    }
}