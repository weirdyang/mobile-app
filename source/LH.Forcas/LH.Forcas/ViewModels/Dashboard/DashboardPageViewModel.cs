namespace LH.Forcas.ViewModels.Dashboard
{
    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;

        public DashboardPageViewModel()
        {
            this.DummyProp = new Dummy();
        }

        public Dummy DummyProp { get; set; }

        public string TestProp
        {
            get { return this.testProp; }
            set
            {
                this.testProp = value;
                this.OnPropertyChanged();
            }
        }

        public string this[int key]
        {
            get
            {
                if (key == 0)
                {
                    return "000000000";
                }

                return "111111111111111";
            }
        }

        public class Dummy
        {
            public string this[int key]
            {
                get
                {
                    if (key == 0)
                    {
                        return "AAAAAAAAAAA";
                    }

                    return "BBBBBBBBBBB";
                }
            }
        }
    }
}