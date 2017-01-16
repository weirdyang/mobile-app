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
        private AddAccountFlowState flowState;

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            this.flowState = parameters.GetNavigationParameter<AddAccountFlowState>(NavigationExtensions.FlowStateParameterName);
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

        protected override void OnItemSelected(string accountType)
        {
            this.flowState.Account = this.CreateAccount(accountType);
            this.flowState.NavigateToSecondStep();
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
