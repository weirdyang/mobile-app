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
            return this.roamingDataRepository.GetAll<Account>();
        }

        public Account GetAccount(Guid id)
        {
            return this.roamingDataRepository.GetOneById<Account>(id);
        }

        public void DeleteAccount(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}