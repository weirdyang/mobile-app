namespace LH.Forcas.ViewModels.Dashboard
{
    using FluentValidation;

    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;

        public DashboardPageViewModel()
        {
            this.Validator = new DashboardValidator();
        }

        public string TestProp
        {
            get { return this.testProp; }
            set { this.SetProperty(ref this.testProp, value); }
        }

        public string this[int i]
        {
            get
            {
                return "AAAAA";
            }
        }

        private class DashboardValidator : AbstractValidator<DashboardPageViewModel>
        {
            public DashboardValidator()
            {
                this.RuleFor(x => x.TestProp).NotEmpty();
            }
        }
    }
}