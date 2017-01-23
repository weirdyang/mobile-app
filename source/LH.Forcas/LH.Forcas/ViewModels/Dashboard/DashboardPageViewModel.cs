namespace LH.Forcas.ViewModels.Dashboard
{
    using System.Collections.Generic;
    using FluentValidation;

    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;
        private DummyItem selected;

        public DashboardPageViewModel()
        {
            this.Validator = new DashboardValidator();
            this.Items = new List<DummyItem>
            {
                new DummyItem { Name = "First Item" },
                new DummyItem { Name = "Second Item" }
            };
        }

        public string TestProp
        {
            get { return this.testProp; }
            set { this.SetProperty(ref this.testProp, value); }
        }

        public IList<DummyItem> Items { get; set; }

        public DummyItem Selected
        {
            get { return this.selected; }
            set
            {
                this.selected = value;
                this.OnPropertyChanged();
            }
        }

        private class DashboardValidator : AbstractValidator<DashboardPageViewModel>
        {
            public DashboardValidator()
            {
                this.RuleFor(x => x.TestProp).NotEmpty();
            }
        }

        public class DummyItem
        {
            public string Name { get; set; }
        }
    }
}