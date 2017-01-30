namespace LH.Forcas.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Integration.Banks;

    public interface IAccountingService
    {
        IEnumerable<Account> GetAccounts();

        void DeleteAccount(Guid id);

        Account GetAccount(Guid id);

        void SaveAccount(Account account);

        Task<IList<RemoteAccountInfo>> GetAvailableRemoteAccounts(string bankId);
    }
}