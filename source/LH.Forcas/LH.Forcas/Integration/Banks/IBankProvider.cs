namespace LH.Forcas.Integration.Banks
{
    using System;
    using Domain.UserData.Authorization;
    using LH.Forcas.Domain.UserData;

    public interface IBankProvider
    {
        void Initialize(BankAuthorizationBase authorizationBase);

        RemoteAccountInfo[] FetchAccounts();

        Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime);
    }
}