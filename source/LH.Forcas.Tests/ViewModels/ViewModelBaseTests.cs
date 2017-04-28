using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LH.Forcas.ViewModels;
using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels
{
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

            [Test]
            public void ShouldThrowAndClearIsBusyWhenActionFails()
            {
                var viewModel = new TestViewModel();
                Assert.IsFalse(viewModel.IsBusy);

                var task = viewModel.RunFailingAction();

                var ex = Assert.Throws<AggregateException>(() => task.Wait());

                var flatEx = ex.Flatten();
                Assert.AreEqual(1, flatEx.InnerExceptions.Count);
                Assert.AreEqual(TestViewModel.ExceptionMessage, flatEx.InnerExceptions.Single().Message);
                Assert.IsFalse(viewModel.IsBusy);
            }

            [Test]
            public void ShouldThrowAndClearIsBusyWhenTaskFails()
            {
                var viewModel = new TestViewModel();
                Assert.IsFalse(viewModel.IsBusy);

                Console.WriteLine("Starting task");
                var task = viewModel.RunFailingAsTask();

                var ex = Assert.Throws<AggregateException>(() => task.Wait());

                var flatEx = ex.Flatten();
                Assert.AreEqual(1, flatEx.InnerExceptions.Count);
                Assert.AreEqual(TestViewModel.ExceptionMessage, flatEx.InnerExceptions.Single().Message);
                Assert.IsFalse(viewModel.IsBusy);
            }
        }

        private class TestViewModel : ViewModelBase
        {
            public const string ExceptionMessage = "Error occured...";

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

            public Task RunFailingAction()
            {
                return this.RunAsyncWithBusyIndicator(() => { throw new Exception(ExceptionMessage); });
            }

            public Task RunFailingAsTask()
            {
                return this.RunAsyncWithBusyIndicator(this.AsyncFailing);
            }

            public void FinishLongRunningLogic()
            {
                this.resetEvent.Set();
            }

            #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            private async Task AsyncLongRunningLogic()
            {
                this.resetEvent.WaitOne();
                // return Task.FromResult(0);
            }

            private async Task AsyncFailing()
            {
                throw new Exception(ExceptionMessage);
            }
            #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        }
    }
}
