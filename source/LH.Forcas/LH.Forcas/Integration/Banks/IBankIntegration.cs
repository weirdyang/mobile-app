namespace LH.Forcas.Integration.Banks
{
    using System;
    using LH.Forcas.Domain.UserData;

    public interface IBankIntegration
    {
        void Initialize(BankAuthorization authorization);

        Account[] FetchAccounts();

        Transaction[] FetchTransactions(Account account, DateTime lastDownloadTime);
    }
}