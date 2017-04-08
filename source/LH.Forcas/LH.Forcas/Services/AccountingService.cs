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
        private readonly IUserDataRepository userDataRepository;

        public AccountingService(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }

        #region Accounts

        public IEnumerable<Account> GetAccounts()
        {
            return this.userDataRepository.GetAll<Account>();
        }

        public Account GetAccount(Guid id)
        {
            return this.userDataRepository.GetOneById<Account>(id);
        }

        public void SaveAccount(Account account)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(Guid id)
        {
            this.userDataRepository.Delete<Account>(id);
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
            return this.userDataRepository.GetAll<Category>();
        }

        public void DeleteCategory(Guid categoryId, Guid? moveTransactionsIntoCategoryId)
        {
            // TODO: Change category of all transactions in the category
            this.userDataRepository.Delete<Category>(categoryId);
        }

        public void SaveCategory(Category category)
        {
            this.userDataRepository.Insert(category);
        }

        #endregion
    }
}