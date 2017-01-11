namespace LH.Forcas.Tests.ViewModels.Accounts
{
    using Forcas.ViewModels.Accounts;
    using Moq;
    using NUnit.Framework;
    using Prism.Navigation;

    public abstract class AccountsListPageViewModelTests
    {
        [SetUp]
        public void Setup()
        {
            this.NavigationServiceMock = new Mock<INavigationService>();

            this.ViewModel = new AccountsListPageViewModel(
                this.NavigationServiceMock.Object);
        }

        protected Mock<INavigationService> NavigationServiceMock;
        protected AccountsListPageViewModel ViewModel;

        [TestFixture]
        public class NavigationTests : AccountsListPageViewModelTests
        {
            [Test]
            public void ShouldNavigateWhenCommandIsExecuted()
            {
                this.NavigationServiceMock.Setup(x => x.NavigateAsync(It.Is<string>(uri => uri.Contains("New")), null, false, true));

                this.ViewModel.NavigateToAddAccount.Execute(null);

                this.NavigationServiceMock.VerifyAll();
            }
        }

        /*
         * Refresh commands reloads data
         * Data loaded when the view is navigated to
         * When item is selected -> navigate to the detail
         * Delete item tests -> check the scenario with full confirm (nav service has methods)
         */
    }
}
