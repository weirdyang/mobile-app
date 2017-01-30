namespace LH.Forcas.Integration.Banks
{
    using System;
    using System.Collections.Generic;

    public interface IBankProviderCatalog
    {
        void Initialize(IEnumerable<Type> providerTypes = null);

        Type GetAuthorizationType(string bankId);

        IBankProvider GetIntegrationProvider(string bankId);
    }
}