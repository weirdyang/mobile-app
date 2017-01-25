namespace LH.Forcas.Tests.ViewModels
{
    using System.Threading;
    using System.Threading.Tasks;
    using Forcas.ViewModels;
    using NUnit.Framework;

    [TestFixture]
    public class ViewModelBaseTests
    {
        [TestFixture]
        public class IsBusyTests
        {
            [Test]
            public void ShouldSetIsBusyWhenRunningWithBusyIndicator()
            {
                var viewModel = new TestViewModel();
                Assert.IsFalse(viewModel.IsBusy);

                var longRunning = viewModel.RunLongRunningLogic();
                Assert.IsTrue(viewModel.IsBusy);

                viewModel.ResetEvent.Reset(); // Simulates long running stuff is finished

                longRunning.Wait();
                Assert.IsFalse(viewModel.IsBusy);
            }

            [Test]
            public void ShouldSetIsBusyWhenRunningWithBusyIndicatorAsTask()
            {
                var viewModel = new TestViewModel();               
                Assert.IsFalse(viewModel.IsBusy);

                var longRunning = viewModel.RunLongRunningLogicAsTask();
                Assert.IsTrue(viewModel.IsBusy);

                viewModel.ResetEvent.Reset(); // Simulates long running stuff is finished

                longRunning.Wait();
                Assert.IsFalse(viewModel.IsBusy);
            }
        }

        private class TestViewModel : ViewModelBase
        {
            public readonly ManualResetEvent ResetEvent = new ManualResetEvent(true);

            public Task RunLongRunningLogic()
            {
                return this.RunAsyncWithBusyIndicator(() =>
                                          {
                                              this.ResetEvent.WaitOne();
                                          });
            }

            public Task RunLongRunningLogicAsTask()
            {
                return this.RunAsyncWithBusyIndicator(this.AsyncLongRunningLogic());
            }

            private Task AsyncLongRunningLogic()
            {
                this.ResetEvent.WaitOne();
                return Task.FromResult(0);
            }
        }        
    }
}
