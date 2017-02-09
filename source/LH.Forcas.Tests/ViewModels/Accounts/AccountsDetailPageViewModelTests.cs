namespace LH.Forcas.Tests.ViewModels.Accounts
{
    using System;
    using System.Linq;
    using Forcas.Integration.Banks;
    using LH.Forcas.Domain.RefData;
    using LH.Forcas.Domain.UserData;
    using LH.Forcas.Extensions;
    using LH.Forcas.Services;
    using LH.Forcas.ViewModels.Accounts;
    using Moq;
    using NUnit.Framework;
    using Prism.Navigation;
    using Prism.Services;

    [TestFixture]
    public class AccountsDetailPageViewModelTests
    {
        // TODO: Update init tests -> editability rules
        // Bank accounts -> only name
        // Cash accounts -> only name

        protected Guid AccountId;
        protected Mock<IRefDataService> RefDataServiceMock;
        protected Mock<IAccountingService> AccountingServiceMock;
        protected Mock<IPageDialogService> PageDialogServiceMock;
        protected Mock<INavigationService> NavigationServiceMock;
        protected Mock<IUserSettingsService> UserSettingsServiceMock;
        protected AccountsDetailPageViewModel ViewModel;

        [SetUp]
        public virtual void Setup()
        {
            this.AccountId = Guid.NewGuid();

            this.RefDataServiceMock = new Mock<IRefDataService>();
            this.AccountingServiceMock = new Mock<IAccountingService>();
            this.PageDialogServiceMock = new Mock<IPageDialogService>();
            this.NavigationServiceMock = new Mock<INavigationService>();
            this.UserSettingsServiceMock = new Mock<IUserSettingsService>();

            this.ViewModel = new AccountsDetailPageViewModel(
                this.AccountingServiceMock.Object,
                this.RefDataServiceMock.Object,
                this.UserSettingsServiceMock.Object,
                this.NavigationServiceMock.Object,
                this.PageDialogServiceMock.Object);

            var banks = new[]
            {
                new Bank { BankId = "TestBank", Name = "Test Bank Name", CountryId = "CZ" },
                new Bank { BankId = "OtherBank", Name = "Test Bank Name", CountryId = "GB" }
            };

            var countries = new[]
            {
                new Country { CountryId = "GB", DefaultCurrencyId = "GBP" },
                new Country { CountryId = "CZ", DefaultCurrencyId = "CZK" }
            };

            var currencies = new[]
            {
                new Currency { CurrencyId  = "CZK", IsActive = true, DisplayFormat = "{0} Kč" },
                new Currency { CurrencyId  = "GBP", IsActive = true, DisplayFormat = "£{0}" },
            };

            this.UserSettingsServiceMock.Setup(x => x.Settings).Returns(new UserSettings { DefaultCountryId = "CZ" });
            this.RefDataServiceMock.Setup(x => x.GetBanks()).Returns(banks);
            this.RefDataServiceMock.Setup(x => x.GetCountries()).Returns(countries);
            this.RefDataServiceMock.Setup(x => x.GetCurrencies()).Returns(currencies);
        }

        [TestFixture]
        public class FullScenarioTests : AccountsDetailPageViewModelTests
        {
            [Test]
            public void EditAndSaveBankAccountTest()
            {
                var differentAccount = this.GetDifferentBankAccount();

                this.NavigationServiceMock.Setup(x => x.GoBackAsync(null, null, true)).ReturnsAsync(true);

                this.AccountingServiceMock.Setup(x => x.SaveAccount(It.Is<Account>(acc => this.CompareAccounts(differentAccount, acc, false))));
                this.AccountingServiceMock.Setup(x => x.GetAvailableRemoteAccounts(It.IsAny<string>())).ReturnsAsync(new[] { this.GetRemoteAccount() });

                this.NavigateToEdit(this.GetValidBankAccount());

                var differentAccountBank = this.ViewModel.Banks.Single(x => x.BankId == differentAccount.BankId);

                this.ViewModel.AccountName = differentAccount.Name;
                this.ViewModel.SelectedCountry = this.ViewModel.Countries.Single(x => x.CountryId == differentAccountBank.CountryId);
                this.ViewModel.SelectedBank = differentAccountBank;

                Assert.IsTrue(this.ViewModel.SaveCommand.CanExecute());
                this.ViewModel.SaveCommand.Execute();

                this.NavigationServiceMock.VerifyAll();
                this.AccountingServiceMock.VerifyAll();
            }

            [Test]
            public void CreateAndSaveBankAccountTest()
            {
                var validAccount = this.GetValidBankAccount();
                this.NavigationServiceMock.Setup(x => x.GoBackAsync(null, null, true)).ReturnsAsync(true);

                this.AccountingServiceMock.Setup(x => x.SaveAccount(It.Is<Account>(acc => this.CompareAccounts(validAccount, acc, true))));
                this.AccountingServiceMock.Setup(x => x.GetAvailableRemoteAccounts(It.IsAny<string>())).ReturnsAsync(new[] { this.GetRemoteAccount() });

                this.ViewModel.OnNavigatedTo(null);

                var bank = this.ViewModel.Banks.Single(x => x.BankId == validAccount.BankId);

                this.ViewModel.SelectedAccountType = typeof(BankAccount);
                this.ViewModel.AccountName = validAccount.Name;
                this.ViewModel.SelectedCountry = this.ViewModel.Countries.Single(x => x.CountryId == bank.CountryId);
                this.ViewModel.SelectedBank = bank;
                this.ViewModel.WaitUntilNotBusy();

                this.ViewModel.SelectedRemoteAccount = this.ViewModel.RemoteAccounts.First();

                Assert.IsTrue(this.ViewModel.SaveCommand.CanExecute(), this.ViewModel.ValidationResults.BuildErrorsString());
                this.ViewModel.SaveCommand.Execute();

                this.AccountingServiceMock.VerifyAll();
                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void EditAndSaveCashAccountTest()
            {
                var differentAccount = this.GetDifferentCashAccount();
                this.AccountingServiceMock.Setup(x => x.SaveAccount(It.Is<Account>(acc => this.CompareAccounts(differentAccount, acc, false))));

                this.NavigateToEdit(this.GetValidCashAccount());

                this.ViewModel.AccountName = differentAccount.Name;
                this.ViewModel.SelectedCurrency = this.ViewModel.Currencies.Single(x => x.CurrencyId == differentAccount.CurrencyId);

                Assert.IsTrue(this.ViewModel.SaveCommand.CanExecute(), this.ViewModel.ValidationResults.BuildErrorsString());
                this.ViewModel.SaveCommand.Execute();

                this.AccountingServiceMock.VerifyAll();
            }

            [Test]
            public void CreateAndSaveCashAccountTest()
            {
                var validAccount = this.GetValidCashAccount();

                this.AccountingServiceMock.Setup(x => x.SaveAccount(It.Is<Account>(acc => this.CompareAccounts(validAccount, acc, true))));

                this.ViewModel.OnNavigatedTo(null);

                this.ViewModel.SelectedAccountType = typeof(CashAccount);
                this.ViewModel.AccountName = validAccount.Name;
                this.ViewModel.SelectedCurrency = this.ViewModel.Currencies.Single(x => x.CurrencyId == validAccount.CurrencyId);

                Assert.IsTrue(this.ViewModel.SaveCommand.CanExecute(), this.ViewModel.ValidationResults.BuildErrorsString());
                this.ViewModel.SaveCommand.Execute();

                this.AccountingServiceMock.VerifyAll();
            }

            [Test]
            public void EditAccountWithInvalidValueTest()
            {
                this.NavigateToEdit(this.GetValidCashAccount());
                this.ViewModel.AccountName = null;

                Assert.IsFalse(this.ViewModel.SaveCommand.CanExecute());
            }

            [Test]
            public void EditAccountSaveFailsTest()
            {
                this.AccountingServiceMock.Setup(x => x.SaveAccount(It.IsAny<Account>())).Throws<Exception>();
                this.PageDialogServiceMock.SetupAlert();

                this.NavigateToEdit(this.GetValidCashAccount());
                this.ViewModel.AccountName = "Different Name";

                Assert.IsTrue(this.ViewModel.SaveCommand.CanExecute());
                this.ViewModel.SaveCommand.Execute();

                this.PageDialogServiceMock.VerifyAll();
                this.AccountingServiceMock.VerifyAll();
            }

            // Cash and Bank
            // Full & valid save (edit)
            // Full & valid save (new)
            // On save -> validate
            // Handle save failure
            // Handle init failure
        }

        [TestFixture]
        public class InitializationTests : AccountsDetailPageViewModelTests
        {
            [Test]
            public void ShouldLoadAccountTypes()
            {
                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotNull(this.ViewModel.AccountTypes);
                Assert.AreEqual(2, this.ViewModel.AccountTypes.Count);
            }

            [Test]
            public void ShouldLoadRefData()
            {
                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotNull(this.ViewModel.Banks);
                Assert.AreEqual(2, this.ViewModel.Banks.Count);

                Assert.IsNotNull(this.ViewModel.Currencies);
                Assert.AreEqual(2, this.ViewModel.Currencies.Count);

                Assert.IsNotNull(this.ViewModel.Countries);
                Assert.AreEqual(2, this.ViewModel.Countries.Count);
            }

            [Test]
            public void PreferedCountryShouldBeSelectedWhenCreatingAccount()
            {
                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotNull(this.ViewModel.SelectedCountry);
                Assert.AreEqual("CZ", this.ViewModel.SelectedCountry.CountryId);
            }

            [Test]
            public void ShouldSwitchToNewAccountModeIfNavigatedWithoutParams()
            {
                this.ViewModel.OnNavigatedTo(null);

                Assert.IsNotEmpty(this.ViewModel.Title);
                Assert.IsTrue(string.IsNullOrEmpty(this.ViewModel.AccountName));
                Assert.IsTrue(this.ViewModel.CanEditAccountType);
            }

            [Test]
            public void ShouldLoadExistingAccountIfNavigatedWithParams()
            {
                var editedAccount = this.GetValidBankAccount();

                this.NavigateToEdit(editedAccount);

                Assert.AreEqual(editedAccount.Name, this.ViewModel.Title);
                Assert.AreEqual(editedAccount.Name, this.ViewModel.AccountName);
                Assert.AreEqual(typeof(BankAccount), this.ViewModel.SelectedAccountType);
                Assert.AreEqual(editedAccount.BankId, this.ViewModel.SelectedBank.BankId);
                Assert.AreEqual(this.ViewModel.SelectedBank.CountryId, this.ViewModel.SelectedCountry.CountryId);

                Assert.IsFalse(this.ViewModel.CanEditAccountType);

                this.AccountingServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldDisplayErrorAndNavigateBackIfAccountNotFound()
            {
                var account = this.GetValidBankAccount();
                var parameters = NavigationExtensions.CreateAccountDetailParameters(account.AccountId);

                this.AccountingServiceMock.Setup(x => x.GetAccount(account.AccountId)).Returns(default(Account));
                this.PageDialogServiceMock.Setup(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnAwaitable();
                this.NavigationServiceMock.Setup(x => x.GoBackAsync(null, null, true)).ReturnsAsync(true);

                this.ViewModel.OnNavigatedTo(parameters);

                this.PageDialogServiceMock.VerifyAll();
                this.NavigationServiceMock.VerifyAll();
            }

            [Test]
            public void ShouldDisplayErrorAndNavigateBackIfInitFails()
            {
                var account = this.GetValidBankAccount();
                var parameters = NavigationExtensions.CreateAccountDetailParameters(account.AccountId);

                this.AccountingServiceMock.Setup(x => x.GetAccount(account.AccountId)).Throws<Exception>();
                this.PageDialogServiceMock.Setup(x => x.DisplayAlertAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnAwaitable();
                this.NavigationServiceMock.Setup(x => x.GoBackAsync(null, null, true)).ReturnsAsync(true);

                this.ViewModel.OnNavigatedTo(parameters);

                this.PageDialogServiceMock.VerifyAll();
                this.NavigationServiceMock.VerifyAll();
            }
        }

        [TestFixture]
        public class ValidationTests : AccountsDetailPageViewModelTests
        {
            [Test]
            public void CashAccountValidationTest()
            {
                this.ViewModel.OnNavigatedTo(null);

                this.ViewModel.TestPropertyValidation(x => x.SelectedAccountType, typeof(CashAccount), null);

                this.ViewModel.SelectedAccountType = typeof(CashAccount);

                this.ViewModel.TestPropertyValidation(x => x.AccountName, "My Account", null);
                this.ViewModel.TestPropertyValidation(x => x.SelectedCurrency, this.ViewModel.Currencies.SingleById("CZK"), null);
            }

            [Test]
            public void BankAccountValidationTest()
            {
                this.ViewModel.OnNavigatedTo(null);

                this.ViewModel.TestPropertyValidation(x => x.SelectedAccountType, typeof(BankAccount), null);

                this.ViewModel.SelectedAccountType = typeof(BankAccount);

                this.ViewModel.TestPropertyValidation(x => x.AccountName, "My Account", null);
                this.ViewModel.TestPropertyValidation(x => x.SelectedCurrency, this.ViewModel.Currencies.First(), null);
                this.ViewModel.TestPropertyValidation(x => x.SelectedBank, this.ViewModel.Banks.First(), null);
                this.ViewModel.TestPropertyValidation(x => x.SelectedCountry, this.ViewModel.Countries.First(), null);

                this.ViewModel.SelectedBank = this.ViewModel.Banks.First();
                this.ViewModel.WaitUntilNotBusy();

                this.ViewModel.TestPropertyValidation(x => x.SelectedRemoteAccount, this.ViewModel.RemoteAccounts.First(), null);
            }
        }

        protected void NavigateToEdit(Account account)
        {
            var parameters = NavigationExtensions.CreateAccountDetailParameters(this.AccountId);

            this.AccountingServiceMock
                .Setup(x => x.GetAccount(this.AccountId))
                .Returns(account);

            this.ViewModel.OnNavigatedTo(parameters);
        }

        private CashAccount GetValidCashAccount()
        {
            return new CashAccount
            {
                Name = "Some Cash Account",
                CurrencyId = "CZK"
            };
        }

        private CashAccount GetDifferentCashAccount()
        {
            return new CashAccount
            {
                Name = "Some Other Cash Account",
                CurrencyId = "GBP"
            };
        }

        private RemoteAccountInfo GetRemoteAccount()
        {
            return new RemoteAccountInfo
            {
                AccountNumber = AccountNumber.FromCzLocal("123/5500"),
                CurrencyId = "CZK"
            };
        }

        private BankAccount GetValidBankAccount()
        {
            return new BankAccount
            {
                AccountNumber = AccountNumber.FromCzLocal("123/5500"),
                BankId = "TestBank",
                CurrencyId = "CZK",
                Name = "My Test Account",
                CurrentBalance = new Amount(0m, "CZK")
            };
        }

        private BankAccount GetDifferentBankAccount()
        {
            return new BankAccount
            {
                AccountNumber = AccountNumber.FromCzLocal("456/5577"),
                BankId = "OtherBank",
                CurrencyId = "GBP",
                Name = "Another Account's Name",
                CurrentBalance = new Amount(0m, "GBP")
            };
        }

        private bool CompareAccounts(Account expected, Account actual, bool ignoreAccountId)
        {
            if (!ignoreAccountId)
            {
                Assert.AreEqual(expected.AccountId, actual.AccountId);
            }

            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.CurrencyId, actual.CurrencyId);

            var expectedBank = expected as BankAccount;
            var actualBank = actual as BankAccount;

            if (expectedBank != null)
            {
                Assert.AreEqual(expectedBank.BankId, actualBank.BankId);
                Assert.AreEqual(expectedBank.AccountNumber, actualBank.AccountNumber);
            }

            return true;
        }
    }
}