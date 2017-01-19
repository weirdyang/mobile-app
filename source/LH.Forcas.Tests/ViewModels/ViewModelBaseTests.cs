namespace LH.Forcas.Tests.ViewModels
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
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
                var viewModel = new TestViewModel(false);
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
                var viewModel = new TestViewModel(false);               
                Assert.IsFalse(viewModel.IsBusy);

                var longRunning = viewModel.RunLongRunningLogicAsTask();
                Assert.IsTrue(viewModel.IsBusy);

                viewModel.ResetEvent.Reset(); // Simulates long running stuff is finished

                longRunning.Wait();
                Assert.IsFalse(viewModel.IsBusy);
            }
        }

        [TestFixture]
        public class ValidationTests
        {
            [Test]
            public void ShouldValidatePropertyWhenValueSet()
            {
                var viewModel = new TestViewModel();
                Assert.IsNull(viewModel.ValidationResults["Property"]);

                viewModel.Property = "Valid value";
                Assert.IsNotNull(viewModel.ValidationResults["Property"]);
                Assert.IsTrue(viewModel.ValidationResults["Property"].IsValid);

                viewModel.Property = null;
                Assert.IsNotNull(viewModel.ValidationResults["Property"]);
                Assert.IsFalse(viewModel.ValidationResults["Property"].IsValid);
            }

            [Test]
            public void ShouldNotAttemptValidationWhenValidatorIsNotDefined()
            {
                var viewModel = new TestViewModel(false);
                viewModel.Property = "Some value";

                Assert.IsNull(viewModel.ValidationResults["Property"]);
            }
        }

        private class TestViewModel : ViewModelBase
        {
            public readonly ManualResetEvent ResetEvent = new ManualResetEvent(true);

            private string property;

            public TestViewModel(bool setValidator = true)
            {
                if (setValidator)
                {
                    this.Validator = new TestViewModelValidator();
                }
            }

            public string Property
            {
                get { return this.property; }
                set { this.SetProperty(ref this.property, value); }
            }

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

        private class TestViewModelValidator : AbstractValidator<TestViewModel>
        {
            public TestViewModelValidator()
            {
                this.RuleFor(x => x.Property).NotEmpty();
            }
        }
    }
}
