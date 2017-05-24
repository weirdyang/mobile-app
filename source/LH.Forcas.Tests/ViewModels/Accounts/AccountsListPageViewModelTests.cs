using System;
using System.Linq;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Services;
using LH.Forcas.ViewModels.Accounts;
using Moq;
using MvvmCross.Core.Navigation;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels.Accounts
{
    [TestFixture]
    public class AccountsListPageViewModelTests
    {
        protected AccountsListViewModel ViewModel;
        protected Mock<IAccountingService> AccountingServiceMock;
        protected Mock<IMvxNavigationService> NavigationServiceMock;
        protected Mock<IUserInteraction> UserInteraction;
        protected Mock<IAnalyticsReporter> AnalyticsReporterMock;

        [SetUp]
        public void Setup()
        {
            this.AccountingServiceMock = new Mock<IAccountingService>();
            this.NavigationServiceMock = new Mock<IMvxNavigationService>();
            this.UserInteraction = new Mock<IUserInteraction>();
            this.AnalyticsReporterMock = new Mock<IAnalyticsReporter>();

            this.ViewModel = new AccountsListViewModel(
                this.NavigationServiceMock.Object,
                this.AccountingServiceMock.Object,
                this.UserInteraction.Object,
                this.AnalyticsReporterMock.Object);

            this.AccountingServiceMock.Setup(x => x.GetAccounts())
                .Returns(new Account[]
                {
                    new CheckingAccount { Id = Guid.NewGuid(), Name = "Checking" },
                    new SavingsAccount { Id = Guid.NewGuid(), Name = "Savings" },
                    new CheckingAccount { Id = Guid.NewGuid(), Name = "Deleted Account", IsDeleted = true }
                });
        }

        public class WhenNavigatingTo : AccountsListPageViewModelTests
        {
            [Test]
            public async Task ShouldLoadActiveAccountsWhenNavigatedTo()
            {
                await this.ViewModel.AppearingAsync();
                this.AccountingServiceMock.VerifyAll();

                Assert.False(this.ViewModel.AccountGroups.SelectMany(x => x).Any(x => x.IsDeleted));
                Assert.False(this.ViewModel.NoAccountsTextDisplayed);
            }

            [Test]
            public async Task ShouldDisplayNoAccountsTestsWhenNoAccountsExist()
            {
                this.AccountingServiceMock.Reset();
                this.AccountingServiceMock.Setup(x => x.GetAccounts()).Returns(() => null);

                await this.ViewModel.AppearingAsync();
                this.AccountingServiceMock.VerifyAll();

                Assert.True(this.ViewModel.NoAccountsTextDisplayed);
            }
        }

        public class WhenNavigatingAway : AccountsListPageViewModelTests
        {
            [Test]
            public async Task ShouldNavigateWhenNavigateToAddCommandIsExecuted()
            {
                this.NavigationServiceMock.Setup(x => x.Navigate<AccountsDetailPageViewModel>()).ReturnsAwaitable();

                await this.ViewModel.AppearingAsync();

                this.ViewModel.NavigateToAddAccountCommand.ExecuteAsync().Wait();

                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public async Task ShouldNavigateWhenNavigateToDetailCommandIsExecuted()
            {
                await this.ViewModel.AppearingAsync();

                var accountToNavigate = this.ViewModel.AccountGroups.First().First();

                this.NavigationServiceMock.Setup(x => x.Navigate<AccountsDetailPageViewModel, Account>(accountToNavigate)).ReturnsAwaitable();

                this.ViewModel.NavigateToAccountDetailCommand.ExecuteAsync(accountToNavigate).Wait();

                this.NavigationServiceMock.VerifyAll();
            }
        }
        
        public class WhenDeletingAccount : AccountsListPageViewModelTests
        {
            [Test]
            public async Task ShouldDisplayConfirmDialog()
            {
                this.SetupDeleteConfirm(false);

                await this.ViewModel.AppearingAsync();
                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(this.ViewModel.AccountGroups.First().First());

                this.UserInteraction.VerifyAll();
            }

            [Test]
            public async Task ShouldNotDeleteAccountIfConfirmWasRespondedWithNo()
            {
                this.SetupDeleteConfirm(false);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>()));

                await this.ViewModel.AppearingAsync();

                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(this.ViewModel.AccountGroups.First().First());

                this.UserInteraction.VerifyAll();
                this.AccountingServiceMock.Verify(x => x.DeleteAccount(It.IsAny<Guid>()), Times.Never);
            }

            [Test]
            public async Task ShouldDeleteAccountIfConfirmed()
            {
                await this.ViewModel.AppearingAsync();

                var accountId = this.ViewModel.AccountGroups.First().First().Id;
                
                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.Is<Guid>(id => id == accountId)));
                
                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(this.ViewModel.AccountGroups.First().First());
                
                this.UserInteraction.VerifyAll();
                this.AccountingServiceMock.VerifyAll();
            }

            [Test]
            public async Task ShouldNotifyUserOnFailure()
            {
                this.SetupDeleteConfirm(true);
                this.UserInteraction.SetupAlert();
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws<Exception>();

                await this.ViewModel.AppearingAsync();

                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(this.ViewModel.AccountGroups.First().First());

                this.UserInteraction.VerifyAll();
            }

            [Test]
            public async Task ShouldReportFailureToAnalytics()
            {
                this.SetupDeleteConfirm(true);
                this.AccountingServiceMock.Setup(x => x.DeleteAccount(It.IsAny<Guid>())).Throws<Exception>();

                this.AnalyticsReporterMock.Setup(x => x.ReportHandledException(It.IsAny<Exception>(), null));

                await this.ViewModel.AppearingAsync();

                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(this.ViewModel.AccountGroups.First().First());

                this.AnalyticsReporterMock.VerifyAll();
            }

            [Test]
            public async Task ShouldIgnoreCommandCalledWithoutParameter()
            {
                await this.ViewModel.AppearingAsync();
                await this.ViewModel.DeleteAccountCommand.ExecuteAsync(null);

                this.AccountingServiceMock.VerifyAll();
                this.UserInteraction.VerifyAll();
            }
        }

        protected void SetupDeleteConfirm(bool result)
        {
            this.UserInteraction.Setup(x => x.ConfirmAsync(
                                                It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>()))
                                     .ReturnsAsync(result);
        }
    }
}
