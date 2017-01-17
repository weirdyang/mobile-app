namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.UserData;
    using Extensions;
    using Localization;
    using Prism.Navigation;

    public class AccountsAddTypeSelectionPageViewModel : ItemSelectionViewModelBase<string>
    {
        private readonly INavigationService navigationService;

        private AddAccountFlow flow;

        public AccountsAddTypeSelectionPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.flow = parameters.GetNavigationParameter<AddAccountFlow>(NavigationExtensions.FlowParameterName);
        }

        protected override Task<IEnumerable<string>> GetSelectionItems()
        {
            var result = new[]
            {
                nameof(AppResources.AccountType_Cash),
                nameof(AppResources.AccountType_Bank),
                nameof(AppResources.AccountType_Loan)
            };

            return Task.FromResult<IEnumerable<string>>(result);
        }

        protected override async void OnItemSelected(string accountType)
        {
            this.flow.Account = this.CreateAccount(accountType);
            await this.flow.NavigateNext(AddAccountFlow.Steps.AccountTypeSelection, this.navigationService);
        }

        private Account CreateAccount(string accountType)
        {
            switch (accountType)
            {
                case nameof(AppResources.AccountType_Cash):
                    return new CashAccount();

                case nameof(AppResources.AccountType_Bank):
                    return new BankAccount();

                case nameof(AppResources.AccountType_Loan):
                    return new LoanAccount();

                default:
                    throw new NotSupportedException($"The account type {accountType} is not supported.");
            }
        }
    }
}
