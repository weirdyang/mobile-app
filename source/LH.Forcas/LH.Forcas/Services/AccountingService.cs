using System.Linq;
using LH.Forcas.Banking;

namespace LH.Forcas.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Storage;

    public class AccountingService : IAccountingService
    {
        private readonly IUserDataRepository userDataRepository;

        public AccountingService(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return this.userDataRepository.GetAccounts();
        }

        public Account GetAccount(Guid id)
        {
            // return this.userDataRepository.GetOneById<Account>(id);
            throw new NotImplementedException();
        }

        public void SaveAccount(Account account)
        {
            this.userDataRepository.SaveAccount(account);
        }

        public void DeleteAccount(Guid id)
        {
            this.userDataRepository.SoftDeleteAccount(id);
        }

        public Task<IList<RemoteAccountInfo>> GetAvailableRemoteAccounts(string bankId)
        {
            // TODO: Filter out accounts which already exist (by account number whatever...)

            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
            //return this.userDataRepository.GetAll<Category>();
        }

        public void DeleteCategory(Guid categoryId, Guid? moveTransactionsIntoCategoryId)
        {
            // TODO: Change category of all transactions in the category
            throw new NotImplementedException();
            // this.userDataRepository.Delete<Category>(categoryId);
        }

        public void SaveCategory(Category category)
        {
            throw new NotImplementedException();
            // this.userDataRepository.Insert(category);
        }
    }
}