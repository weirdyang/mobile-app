namespace LH.Forcas.Services
{
    using System;
    using System.Collections.Generic;
    using Domain.UserData;
    using Storage;

    public class AccountingService : IAccountingService
    {
        private readonly IRoamingDataRepository roamingDataRepository;

        public AccountingService(IRoamingDataRepository roamingDataRepository)
        {
            this.roamingDataRepository = roamingDataRepository;
        }

        public IEnumerable<Account> GetAccounts()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAccount(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}