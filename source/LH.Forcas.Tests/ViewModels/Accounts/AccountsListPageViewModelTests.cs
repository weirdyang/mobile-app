namespace LH.Forcas.Tests.ViewModels.Accounts
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Forcas.Domain.UserData;
    using Forcas.Extensions;
    using Forcas.Services;
    using Forcas.ViewModels.Accounts;
    using Moq;
    using NUnit.Framework;
    using Prism.Navigation;
    using Prism.Services;

    [TestFixture]
    public class AccountsListPageViewModelTests
    {
        protected AccountsListPageViewModel ViewModel;
        protected Mock<IAccountingService> AccountingServiceMock;
        protected Mock<INavigationService> NavigationServiceMock;
        protected Mock<IPageDialogService> DialogServiceMock;

        [SetUp]
        public void Setup()
        {
            this.AccountingServiceMock = new Mock<IAccountingService>();
            this.NavigationServiceMock = new Mock<INavigationService>();
            this.DialogServiceMock = new Mock<IPageDialogService>();

            this.ViewModel = new AccountsListPageViewModel(
                this.AccountingServiceMock.Object,
                this.NavigationServiceMock.Object,
                this.DialogServiceMock.Object);

            this.AccountingServiceMock.Setup(x => x.GetAccounts())
                .Returns(new[]
                         {
                             new BankAccount { AccountId = Guid.NewGuid(), Name = "Checking" },
                             new BankAccount { AccountId = Guid.NewGuid(), Name = "Savings" }
                         });
        }

        [TestFixture]
        public class NavigationTests : AccountsListPageViewModelTests
        {
            [Test]
            public async Task ShouldNavigateWhenNavigateToAddCommandIsExecuted()
            {
                this.NavigationServiceMock.Setup(x => x.NavigateAsync(It.Is<string>(uri => uri.Contains("Detail")), null, null, true)).ReturnAwaitable();

                this.NavigateTo();
                await this.ViewModel.NavigateToAddAccountCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldNavigateWhenNavigateToDetailCommandIsExecuted()
            {
                this.NavigateTo();
                var accountToNavigate = this.ViewModel.Accounts.First().First();

                this.NavigationServiceMock.Setup(x => x.NavigateAsync(
                    It.Is<string>(uri => uri.ToString().Contains("Detail")),
                    It.Is<NavigationParameters>(p => p.HasParameter(NavigationExtensions.AccountIdParameterName, accountToNavigate.AccountId)),
                    null, 
                    true)).ReturnAwaitable();

                this.ViewModel.NavigateToAccountDetailCommand.Execute(accountToNavigate);

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldLoadAccountsWhenNavigatedTo()
            {
                this.NavigateTo();
                this.AccountingServiceMock.VerifyAll();
            }
        }
        
        [TestFixture]
        public class DeleteAccountTests : AccountsListPageViewModelTests
        {
            [Test]
            public void ShouldDisplayConfirmDialog()
            {
                this.SetupDeleteConfirm(false);

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.Accounts.First().First());

                this.DialogServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldNotDeleteAccountIfConfirmWasRespondedWithNo()
            {
                this.SetupDeleteConfirm(false);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.Accounts.First().First());

                this.DialogServiceMock.VerifyAll();
                this.AccountingServiceMock.Verify(x => x.DeleteAccount(It.IsAny<Guid>()), Times.Never);
            }

            [Test]
            public void ShouldDeleteAccountIfConfirmed()
            {
                this.NavigateTo();
                var accountId = this.ViewModel.Accounts.First().First().AccountId;

                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.Is<Guid>(id => id == accountId)));

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.Accounts.First().First());

                this.DialogServiceMock.VerifyAll();
                this.AccountingServiceMock.Verify(x => x.DeleteAccount(It.Is<Guid>(id => id == accountId)), Times.Once);
                this.AccountingServiceMock.Verify(x => x.GetAccounts(), Times.Exactly(2));
            }

            [Test]
            public void ShouldNotifyUserOnFailure()
            {
                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws<Exception>();

                this.DialogServiceMock.Setup(x => x.DisplayAlertAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>(),
                    It.IsAny<string>()));

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.Accounts.First().First());

                this.DialogServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldIgnoreCommandCalledWithoutParameter()
            {
                this.NavigateTo();
                this.ViewModel.DeleteAccountCommand.Execute(null);

                this.AccountingServiceMock.VerifyAll();
                this.DialogServiceMock.VerifyAll();
            }
        }
        /*
         * Refresh commands reloads data and clears selection
         * Data loaded when the view is navigated to
         */

        protected void SetupDeleteConfirm(bool result)
        {
            this.DialogServiceMock.Setup(x => x.DisplayAlertAsync(
                                                It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>()))
                                     .ReturnsAsync(result);
        }

        protected void NavigateTo()
        {
            this.ViewModel.OnNavigatedTo(null);
            this.ViewModel.CurrentBackgroundTask?.Wait();
        }
    }
}
