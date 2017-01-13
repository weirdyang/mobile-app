namespace LH.Forcas.Tests.ViewModels
{
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
            public void ShouldSetIsBusyWhenInBusySection()
            {
                var viewModel = new TestViewModel(false);
                Assert.IsFalse(viewModel.IsBusy);

                using (viewModel.StartBusyIndicator())
                {
                    Assert.IsTrue(viewModel.IsBusy);
                }

                Assert.IsFalse(viewModel.IsBusy);
            }

            [Test]
            public void ShouldKeepBusyWhenExitingNestedSection()
            {
                var viewModel = new TestViewModel(false);

                using (viewModel.StartBusyIndicator())
                {
                    using (viewModel.StartBusyIndicator())
                    {
                        Assert.IsTrue(viewModel.IsBusy);
                    }

                    Assert.IsTrue(viewModel.IsBusy);
                }

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

            public new BusyIndicatorSection StartBusyIndicator()
            {
                return base.StartBusyIndicator();
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
