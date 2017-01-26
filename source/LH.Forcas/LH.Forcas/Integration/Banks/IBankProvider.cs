namespace LH.Forcas.Integration.Banks
{
    using System;
    using Domain.UserData.Authorization;
    using LH.Forcas.Domain.UserData;

    public interface IBankProvider
    {
        void Initialize(BankAuthorizationBase authorizationBase);

        Account[] FetchAccounts();

        Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime);
    }
}