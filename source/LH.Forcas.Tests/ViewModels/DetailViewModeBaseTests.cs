namespace LH.Forcas.Tests.ViewModels
{
    using FluentValidation;
    using Forcas.ViewModels;
    using Moq;
    using NUnit.Framework;
    using Prism.Services;

    [TestFixture]
    public class DetailViewModeBaseTests
    {
        protected Mock<IPageDialogService> PageDialogServiceMock;

        [SetUp]
        public void Setup()
        {
            this.PageDialogServiceMock = new Mock<IPageDialogService>();
        }

        [TestFixture]
        public class ValidationTests : DetailViewModeBaseTests
        {
            [Test]
            public void ShouldValidatePropertyWhenValueSet()
            {
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object);
                viewModel.TestPropertyValidation(x => x.Property, "Valid", null);
            }

            [Test]
            public void ShouldNotAttemptValidationWhenValidatorIsNotDefined()
            {
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object);
                viewModel.Property = "Some value";

                Assert.IsNull(viewModel.ValidationResults["Property"]);
            }

            [Test]
            public void CanSaveShouldValidateAllProperties()
            {
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object);
                
                viewModel.SaveCommand.CanExecute();
                Assert.AreEqual(2, viewModel.ValidationResults.ErrorsCount);
            }
        }

        [TestFixture]
        public class PropertyChangeTests : DetailViewModeBaseTests
        {
            [Test]
            public void ShouldSetIsDirtyWhenValueChanges()
            {
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object, false);

                Assert.IsFalse(viewModel.IsDirty);
                viewModel.Property = "Dummy";

                Assert.IsTrue(viewModel.IsDirty);
            }

            [Test]
            public void ShouldRaiseSaveCommandCanExecuteChangedWhenValueChanged()
            {
                var wasCalled = false;
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object, false);
                viewModel.SaveCommand.CanExecuteChanged += (sender, e) => wasCalled = true;

                Assert.IsFalse(wasCalled);
                viewModel.Property = "Dummy";

                Assert.IsTrue(wasCalled);
            }
        }

        [TestFixture]
        public class NavigationTests : DetailViewModeBaseTests
        {
            [Test]
            public void ShouldShouldDialogIfDirty()
            {
                this.PageDialogServiceMock.SetupAlert(false);
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object, false);

                viewModel.Property = "Dummy";
                Assert.IsFalse(viewModel.CanNavigateAsync(null).Result);
            }

            [Test]
            public void ShouldNavigateIfDirtyAndUserConfirms()
            {
                this.PageDialogServiceMock.SetupAlert(true);
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object, false);

                viewModel.Property = "Dummy";
                Assert.IsTrue(viewModel.CanNavigateAsync(null).Result);
            }

            [Test]
            public void ShouldNotShowDialogIfNotDirty()
            {
                this.PageDialogServiceMock.SetupAlert(false);
                var viewModel = new TestViewModel(this.PageDialogServiceMock.Object, false);

                Assert.IsTrue(viewModel.CanNavigateAsync(null).Result);
            }
        }

        #region Test ViewModel class

        private class TestViewModel : DetailViewModelBase
        {
            private string property;
            private string otherProperty;

            public TestViewModel(IPageDialogService pageDialogService, bool setValidator = true)
                : base(pageDialogService)
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

            public string OtherProperty
            {
                get { return this.otherProperty; }
                set { this.SetProperty(ref this.otherProperty, value); }
            }
        }

        private class TestViewModelValidator : AbstractValidator<TestViewModel>
        {
            public TestViewModelValidator()
            {
                this.RuleFor(x => x.Property).NotEmpty();
                this.RuleFor(x => x.OtherProperty).NotEmpty();
            }
        }

        #endregion
    }
}