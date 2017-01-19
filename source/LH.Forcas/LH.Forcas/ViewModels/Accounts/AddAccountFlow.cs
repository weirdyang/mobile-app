namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.RefData;
    using Domain.UserData;
    using Extensions;
    using Prism.Navigation;
    using Services;
    using Views.Accounts;

    public class AddAccountFlow
    {
        private readonly IAccountingService accountingService;
        private readonly NavigationParameters navigationParameters;

        public AddAccountFlow(IAccountingService accountingService)
        {
            this.accountingService = accountingService;

            this.navigationParameters = new NavigationParameters();
            this.navigationParameters.Add(NavigationExtensions.FlowParameterName, this);
        }

        public Account Account { get; set; }

        public Bank Bank { get; set; }

        public async Task CancelFlow(INavigationService navigationService)
        {
            await navigationService.NavigateToAccounts();
        }

        public async Task NavigateBack(Steps currentStep, INavigationService navigationService)
        {
            if (currentStep == Steps.AccountTypeSelection)
            {
                await this.CancelFlow(navigationService);
            }

            throw new NotImplementedException();
        }

        public async Task NavigateNext(Steps? currentStep, INavigationService navigationService)
        {
            if (!currentStep.HasValue)
            {
                await navigationService.NavigateAsync(nameof(AccountsAddTypeSelectionPage), this.navigationParameters);
                return;
            }

            switch (currentStep.Value)
            {
                case Steps.AccountTypeSelection:
                    if (this.Account is BankAccount)
                    {
                        await navigationService.NavigateAsync(nameof(AccountsAddBankSelectionPage), this.navigationParameters);
                    }
                    else
                    {
                        // -> details page in edit mode (pass the flow as parameter, the VM will save the account)
                    }
                    break;

                case Steps.BankSelection:
                    if (this.ShouldAskForAuthorization())
                    {
                        // GO TO Auth page
                    }
                    else
                    {
                        // GO TO Account selection page
                    }
                    break;

                case Steps.AccountSelection:
                    if (this.Bank.AuthorizationScheme == BankAuthorizationScheme.PerPersona
                        && this.Account == null) // Is connection info null
                    {
                        // Navigate to enter authorization
                    }
                    else
                    {
                        // Finish the flow
                    }
                    break;
            }
        }

        private bool ShouldAskForAuthorization()
        {
            if (this.Bank.AuthorizationScheme == BankAuthorizationScheme.PerAccount)
            {
                return true;
            }

            var accounts = this.accountingService.GetAccounts()
                .OfType<BankAccount>()
                .ToArray();

            if (accounts.Any(x => x.BankId == this.Bank.BankId))
            {
                return false;
            }

            return true;
        }

        public enum Steps
        {
            AccountTypeSelection,
            BankSelection,
            EnterAuthorization,
            AccountSelection
        }
    }
}