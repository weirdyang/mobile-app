namespace LH.Forcas.Services
{
    using System.Collections.Generic;
    using Domain.UserData;

    public interface IAccountingService
    {
        IEnumerable<Account> GetAccounts();
    }
}