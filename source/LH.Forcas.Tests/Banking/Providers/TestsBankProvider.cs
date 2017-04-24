using System;
using LH.Forcas.Banking;
using LH.Forcas.Banking.Providers;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Domain.UserData.Authorization;

namespace LH.Forcas.Tests.Banking.Providers
{
    [BankProviderInfo(typeof(StaticTokenAuthorization), "RB", "UCB")]
    public class TestsBankProvider : IBankProvider
    {
        public void Initialize(BankAuthorizationBase authorizationBase) { }

        public RemoteAccountInfo[] FetchAccounts()
        {
            throw new NotImplementedException();
        }

        public Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime)
        {
            throw new NotImplementedException();
        }
    }
}