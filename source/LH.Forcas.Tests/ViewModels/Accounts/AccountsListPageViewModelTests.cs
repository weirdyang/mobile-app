using LH.Forcas.Analytics;

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
        protected Mock<IAnalyticsReporter> AnalyticsReporterMock;

        [SetUp]
        public void Setup()
        {
            this.AccountingServiceMock = new Mock<IAccountingService>();
            this.NavigationServiceMock = new Mock<INavigationService>();
            this.DialogServiceMock = new Mock<IPageDialogService>();
            this.AnalyticsReporterMock = new Mock<IAnalyticsReporter>();

            this.ViewModel = new AccountsListPageViewModel(
                this.AccountingServiceMock.Object,
                this.NavigationServiceMock.Object,
                this.DialogServiceMock.Object,
                this.AnalyticsReporterMock.Object);

            this.AccountingServiceMock.Setup(x => x.GetAccounts())
                .Returns(new Account[]
                         {
                             new CheckingAccount { Id = Guid.NewGuid(), Name = "Checking" },
                             new SavingsAccount { Id = Guid.NewGuid(), Name = "Savings" }
                         });
        }

        [TestFixture]
        public class WhenNavigatingTo : AccountsListPageViewModelTests
        {
            [Test]
            public void ShouldLoadAccountsWhenNavigatedTo()
            {
                this.NavigateTo();
                this.AccountingServiceMock.VerifyAll();
            }
        }

        [TestFixture]
        public class WhenNavigatingAway : AccountsListPageViewModelTests
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
                var accountToNavigate = this.ViewModel.AccountGroups.First().First();

                this.NavigationServiceMock.Setup(x => x.NavigateAsync(
                    It.Is<string>(uri => uri.ToString().Contains("Detail")),
                    It.Is<NavigationParameters>(p => p.HasParameter(NavigationExtensions.AccountIdParameterName, accountToNavigate.Id)),
                    null, 
                    true)).ReturnAwaitable();

                this.ViewModel.NavigateToAccountDetailCommand.Execute(accountToNavigate);

                this.NavigationServiceMock.VerifyAll();
            }
        }
        
        [TestFixture]
        public class WhenDeletingAccount : AccountsListPageViewModelTests
        {
            [Test]
            public void ShouldDisplayConfirmDialog()
            {
                this.SetupDeleteConfirm(false);

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.AccountGroups.First().First());

                this.DialogServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldNotDeleteAccountIfConfirmWasRespondedWithNo()
            {
                this.SetupDeleteConfirm(false);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.AccountGroups.First().First());

                this.DialogServiceMock.VerifyAll();
                this.AccountingServiceMock.Verify(x => x.DeleteAccount(It.IsAny<Guid>()), Times.Never);
            }

            [Test]
            public void ShouldDeleteAccountIfConfirmed()
            {
                this.NavigateTo();
                var accountId = this.ViewModel.AccountGroups.First().First().Id;

                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.Is<Guid>(id => id == accountId)));

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.AccountGroups.First().First());

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

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.AccountGroups.First().First());

                this.DialogServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldReportFailureToAnalytics()
            {
                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws<Exception>();

                this.AnalyticsReporterMock.Setup(x => x.ReportHandledException(It.IsAny<Exception>(), null));

                this.NavigateTo();

                this.ViewModel.DeleteAccountCommand.Execute(this.ViewModel.AccountGroups.First().First());

                this.AnalyticsReporterMock.VerifyAll();
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
