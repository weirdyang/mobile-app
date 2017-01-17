namespace LH.Forcas.ViewModels.Accounts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.RefData;
    using Extensions;
    using Prism.Navigation;
    using Services;

    public class AccountsAddBankSelectionPageViewModel : ItemSelectionViewModelBase<Bank>
    {
        private AddAccountFlow flow;

        private readonly IRefDataService refDataService;
        private readonly INavigationService navigationService;

        public AccountsAddBankSelectionPageViewModel(IRefDataService refDataService, INavigationService navigationService)
        {
            this.refDataService = refDataService;
            this.navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.flow = parameters.GetNavigationParameter<AddAccountFlow>(NavigationExtensions.FlowParameterName);
        }

        protected override async Task<IEnumerable<Bank>> GetSelectionItems()
        {
            return await this.refDataService.GetBanks();
        }

        protected override async void OnItemSelected(Bank bank)
        {
            this.flow.Bank = bank;
            await this.flow.NavigateNext(AddAccountFlow.Steps.BankSelection, this.navigationService);
        }
    }
}