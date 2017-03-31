namespace LH.Forcas.ViewModels.Dashboard
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using Domain.RefData;
    using Prism.Commands;
    using Prism.Services;
    using Services;

    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;
        private DummyItem selected;
        private bool isSecondSectionVisible;
        private bool isThirdSectionVisible;
        private Country selectedCountry;

        public DashboardPageViewModel(IPageDialogService dialogService, IRefDataService refDataService)
        {
            this.Items = new List<DummyItem>
            {
                new DummyItem { Name = "First Item" },
                new DummyItem { Name = "Second Item" }
            };

            this.DummyCommand = new DelegateCommand(() => dialogService.DisplayAlertAsync("Clicked", "Clicked", "OK"),
                                                    () => this.TestProp.EndsWith("3"))
                .ObservesProperty(() => this.TestProp);


            this.DummyCommand.CanExecuteChanged += (sender, e) => Debug.WriteLine("Can execute changed: {0}", this.DummyCommand.CanExecute(null));

            this.ManyItems = Enumerable.Range(1, 50).Select(x => $"Item {x}").ToArray();

            this.Countries = refDataService.GetCountries();
        }

        public IEnumerable Countries { get; set; }

        public Country SelectedCountry
        {
            get { return this.selectedCountry; }
            set { this.SetProperty(ref this.selectedCountry, value); }
        }

        public ICommand DummyCommand { get; set; }

        public string[] ManyItems { get; set; }

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