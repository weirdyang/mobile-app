namespace LH.Forcas.ViewModels.Dashboard
{
    public class DashboardPageViewModel : ViewModelBase
    {
        private string testProp;

        public DashboardPageViewModel()
        {
        }

        public string TestProp
        {
            get { return this.testProp; }
            set
            {
                this.testProp = value;
                this.OnPropertyChanged();
            }
        }
    }
}