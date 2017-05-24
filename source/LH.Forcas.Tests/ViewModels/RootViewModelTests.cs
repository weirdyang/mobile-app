using LH.Forcas.ViewModels;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels
{
    [TestFixture]
    public class RootViewModelTests
    {
        protected bool RequestSwitchCalled;
        protected RootViewModel ViewModel;

        [SetUp]
        public void Setup()
        {
            this.RequestSwitchCalled = false;
            this.ViewModel = new RootViewModel();
        }

        [TestFixture]
        public class WhenNavigatingTo : RootViewModelTests
        {
            [Test]
            public void ShouldRaiseSwitchRequestWhenNavigatedToWithParameters()
            {
                var parameters = new RootViewModelParams();
                parameters.RequestedTabName = RootViewModel.DashboardTabName;

                this.SubscribeAssertRequestedTabName(RootViewModel.DashboardTabName);
                this.ViewModel.Initialize(parameters);

                Assert.IsTrue(this.RequestSwitchCalled);
            }
        }

        [TestFixture]
        public class WhenSwitchingTabs : RootViewModelTests
        {
            [Test]
            public void ShouldRaiseSwitchRequestWhenDashboardCommandExecuted()
            {
                this.SubscribeAssertRequestedTabName(RootViewModel.DashboardTabName);
                this.ViewModel.SwitchToDashboardCommand.Execute();

                Assert.IsTrue(this.RequestSwitchCalled);
            }

            [Test]
            public void ShouldRaiseSwitchRequestWhenBudgetCommandExecuted()
            {
                this.SubscribeAssertRequestedTabName(RootViewModel.BudgetTabName);
                this.ViewModel.SwitchToBudgetCommand.Execute();

                Assert.IsTrue(this.RequestSwitchCalled);
            }

            [Test]
            public void ShouldRaiseSwitchRequestWhenAddCashCommandExecuted()
            {
                this.SubscribeAssertRequestedTabName(RootViewModel.AddCashTabName);
                this.ViewModel.SwitchToAddCashCommand.Execute();

                Assert.IsTrue(this.RequestSwitchCalled);
            }

            [Test]
            public void ShouldRaiseSwitchRequestWhenAccountsCommandExecuted()
            {
                this.SubscribeAssertRequestedTabName(RootViewModel.AccountsTabName);
                this.ViewModel.SwitchToAccountsCommand.Execute();

                Assert.IsTrue(this.RequestSwitchCalled);
            }

            [Test]
            public void ShouldRaiseSwitchRequestWhenMoreCommandExecuted()
            {
                this.SubscribeAssertRequestedTabName(RootViewModel.MoreTabName);
                this.ViewModel.SwitchToMoreCommand.Execute();

                Assert.IsTrue(this.RequestSwitchCalled);
            }
        }

        protected void SubscribeAssertRequestedTabName(string expectedTabName)
        {
            this.ViewModel.RequestSwitch += (sender, actual) =>
            {
                Assert.AreEqual(expectedTabName, actual);
                this.RequestSwitchCalled = true;
            };
        }
    }
}