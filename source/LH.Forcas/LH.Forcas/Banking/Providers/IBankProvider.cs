using System;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Domain.UserData.Authorization;

namespace LH.Forcas.Banking.Providers
{
    public interface IBankProvider
    {
        void Initialize(BankAuthorizationBase authorizationBase);

        RemoteAccountInfo[] FetchAccounts();

        Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime);
    }
}