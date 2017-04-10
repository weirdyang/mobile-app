using System;

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

                var task = viewModel.RunLongRunningLogic();
                Assert.IsTrue(viewModel.IsBusy);

                viewModel.FinishLongRunningLogic();
                task.Wait();

                Assert.IsFalse(viewModel.IsBusy);
            }

            [Test]
            public void ShouldSetIsBusyWhenRunningWithBusyIndicatorAsTask()
            {
                var viewModel = new TestViewModel();               
                Assert.IsFalse(viewModel.IsBusy);

                Console.WriteLine("Starting task");
                var longRunning = viewModel.RunLongRunningLogicWithTask();
                Assert.IsTrue(viewModel.IsBusy);

                Console.WriteLine("Finishing task");
                viewModel.FinishLongRunningLogic();

                Console.WriteLine("Waiting for the task to exit");
                longRunning.Wait();
                Assert.IsFalse(viewModel.IsBusy);
            }

            [Test]
            public void ShouldClearIsBusyWhenTaskFinishesImmediately()
            {
                var viewModel = new TestViewModel();
                Assert.IsFalse(viewModel.IsBusy);

                Console.WriteLine("Starting task");
                var task = viewModel.RunEmptyAction();

                Console.WriteLine("Waiting for the task to exit");
                task.Wait();
                Assert.IsFalse(viewModel.IsBusy);
            }
        }

        private class TestViewModel : ViewModelBase
        {
            private readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

            public Task RunLongRunningLogic()
            {
                return this.RunAsyncWithBusyIndicator(() =>
                                          {
                                              this.resetEvent.WaitOne();
                                          });
            }

            public Task RunLongRunningLogicWithTask()
            {
                return this.RunAsyncWithBusyIndicator(this.AsyncLongRunningLogic);
            }

            public Task RunEmptyAction()
            {
                return this.RunAsyncWithBusyIndicator(() => { });
            }

            public void FinishLongRunningLogic()
            {
                this.resetEvent.Set();
            }

            private async Task AsyncLongRunningLogic()
            {
                this.resetEvent.WaitOne();
                // return Task.FromResult(0);
            }
        }        
    }
}
