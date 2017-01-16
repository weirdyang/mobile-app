namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Extensions;
    using Prism.Navigation;
    using Services;

    public class AccountsAddBankSelectionPageViewModel : ItemSelectionViewModelBase<Tuple<string, string>>
    {
        private AddAccountFlowState flowState;

        private readonly IRefDataService refDataService;

        public AccountsAddBankSelectionPageViewModel(IRefDataService refDataService)
        {
            this.refDataService = refDataService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.flowState = parameters.GetNavigationParameter<AddAccountFlowState>(NavigationExtensions.FlowStateParameterName);
        }

        protected override async Task<IEnumerable<Tuple<string, string>>> GetSelectionItems()
        {
            var banks = await this.refDataService.GetBanks();

            return banks.Select(x => new Tuple<string, string>(x.BankId, x.Name));
        }

        protected override void OnItemSelected(Tuple<string, string> item)
        {
            var bankAccount = (BankAccount)this.flowState.Account;
            bankAccount.BankId = item.Item1;
        }
    }
}