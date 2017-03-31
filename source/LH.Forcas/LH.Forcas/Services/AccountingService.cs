namespace LH.Forcas.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Integration.Banks;
    using Storage;

    public class AccountingService : IAccountingService
    {
        private readonly IRoamingDataRepository roamingDataRepository;

        public AccountingService(IRoamingDataRepository roamingDataRepository)
        {
            this.roamingDataRepository = roamingDataRepository;
        }

        #region Accounts

        public IEnumerable<Account> GetAccounts()
        {
            return this.roamingDataRepository.GetAll<Account>();
        }

        public Account GetAccount(Guid id)
        {
            return this.roamingDataRepository.GetOneById<Account>(id);
        }

        public void SaveAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(Guid id)
        {
            this.roamingDataRepository.Delete<Account>(id);
        }

        public Task<IList<RemoteAccountInfo>> GetAvailableRemoteAccounts(string bankId)
        {
            // TODO: Filter out accounts which already exist (by account number whatever...)

            throw new NotImplementedException();
        }

        #endregion

        #region Categories

        public IEnumerable<Category> GetCategories()
        {
            return this.roamingDataRepository.GetAll<Category>();
        }

        public void DeleteCategory(Guid categoryId, Guid? moveTransactionsIntoCategoryId)
        {
            // TODO: Change category of all transactions in the category
            this.roamingDataRepository.Delete<Category>(categoryId);
        }

        public void SaveCategory(Category category)
        {
            this.roamingDataRepository.Insert(category);
        }

        #endregion
    }
}