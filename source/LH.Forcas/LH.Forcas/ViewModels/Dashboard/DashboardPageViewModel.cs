namespace LH.Forcas.ViewModels.Dashboard
{
    using System.Collections.Generic;

    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;
        private DummyItem selected;
        private bool isSecondSectionVisible;
        private bool isThirdSectionVisible;

        public DashboardPageViewModel()
        {
            this.Items = new List<DummyItem>
            {
                new DummyItem { Name = "First Item" },
                new DummyItem { Name = "Second Item" }
            };
        }

        public bool IsSecondSectionVisible
        {
            get { return this.isSecondSectionVisible; }
            set
            {
                this.isSecondSectionVisible = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsThirdSectionVisible
        {
            get { return this.isThirdSectionVisible; }
            set
            {
                this.isThirdSectionVisible = value;
                this.OnPropertyChanged();
            }
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

        public class DummyItem
        {
            public string Name { get; set; }
        }
    }
}