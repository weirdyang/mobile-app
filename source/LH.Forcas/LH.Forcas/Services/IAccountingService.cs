namespace LH.Forcas.Services
{
    using System;
    using System.Collections.Generic;
    using Domain.UserData;

    public interface IAccountingService
    {
        IEnumerable<Account> GetAccounts();

        void DeleteAccount(Guid id);
    }
}