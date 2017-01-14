namespace LH.Forcas.Tests.ViewModels.Accounts
{
    using System;
    using System.Linq;
    using LH.Forcas.Domain.RefData;
    using LH.Forcas.Domain.UserData;
    using LH.Forcas.Extensions;
    using LH.Forcas.Services;
    using LH.Forcas.ViewModels.Accounts;
    using Moq;
    using NUnit.Framework;
    using Prism.Navigation;

    [TestFixture]
    public class AccountsDetailPageViewModelTests
    {
        /*
         * NavigatedTo w/o parameters -> new account mode
         * NavigatedTo w/ parameters -> load account 
         * Save command with invalid data -> no navigation, display warning - highlight the fields
         * Save command with valid data -> save
         * Validation rules logic
         */

        protected Mock<IRefDataService> RefDataServiceMock;
        protected Mock<IAccountingService> AccountingServiceMock;
        protected Mock<INavigationService> NavigationServiceMock;
        protected Mock<IUserSettingsService> UserSettingsServiceMock;
        protected AccountsDetailPageViewModel ViewModel;

        [SetUp]
        public void Setup()
        {
            this.RefDataServiceMock = new Mock<IRefDataService>();
            this.AccountingServiceMock = new Mock<IAccountingService>();
            this.NavigationServiceMock = new Mock<INavigationService>();
            this.UserSettingsServiceMock = new Mock<IUserSettingsService>();

            this.ViewModel = new AccountsDetailPageViewModel(
                this.AccountingServiceMock.Object,
                this.RefDataServiceMock.Object,
                this.UserSettingsServiceMock.Object,
                this.NavigationServiceMock.Object);
        }

        [TestFixture]
        public class FullScenarioTests : AccountsDetailPageViewModelTests
        {
            [Test]
            public void EditAccountSuccessfullyTest()
            {
                var account = this.GetValidAccount();

            }

            // Full & valid save (edit)
            // Full & valid save (new)
            // Failing save
            // Invalid save -> should not save
            // On save -> test the account number is set completely
            
            // Account Number should be able to validate itself!
        }

        [TestFixture]
        public class InitializationTests : AccountsDetailPageViewModelTests
        {
            // Failing load

            [Test]
            public void ShouldLoadBanks()
            {
                var banks = new[]
                {
                    new Bank { BankId = "TestBank", Name = "Test Bank Name" }
                };
                
                this.UserSettingsServiceMock.Setup(x => x.CountryCode).Returns("CZE");
                this.RefDataServiceMock.Setup(x => x.GetBanksByCountry("CZE")).ReturnsAsync(banks);

                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotNull(this.ViewModel.Banks);
                Assert.AreEqual(1, this.ViewModel.Banks.Count);
                Assert.AreEqual("TestBank", this.ViewModel.Banks.Keys.Single());
                Assert.AreEqual("Test Bank Name", this.ViewModel.Banks.Values.Single());
            }

            [Test]
            public void ShouldSwitchToNewAccountModeIfNavigatedWithoutParams()
            {
                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotEmpty(this.ViewModel.Title);
            }

            [Test]
            public void ShouldLoadExistingAccountIfNavigatedWithParams()
            {
                var account = this.GetValidAccount();
                var parameters = NavigationExtensions.CreateAccountDetailParameters(account.AccountId);

                this.AccountingServiceMock
                    .Setup(x => x.GetAccount(account.AccountId))
                    .Returns(account);

                this.ViewModel.OnNavigatedTo(parameters);

                Assert.AreEqual(account.Name, this.ViewModel.Title);
                // TODO: Add other properties

                this.AccountingServiceMock.VerifyAll();
            }
        }

        private Account GetValidAccount()
        {
            return new Account
            {
                AccountId = Guid.NewGuid(),
                AccountNumber = AccountNumber.Parse("123/5500"),
                BankId = "RB",
                CurrencyId = "CZK",
                Type = AccountType.Checking,
                Name = "My Test Account",
                CurrentBalance = 1000.98m
            };
        }
    }
}