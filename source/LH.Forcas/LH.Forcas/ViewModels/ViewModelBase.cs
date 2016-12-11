using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;

namespace LH.Forcas.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IConfirmNavigationAsync
    {
        private bool isBusy;
        private BusyIndicatorSection indicatorSection;

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            this.OnNavigatedFromAsync(parameters).Wait();
        }

        public virtual Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            return Task.FromResult(0);
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            this.OnNavigatedToAsync(parameters).Wait();
        }

        public virtual Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            return Task.FromResult(0);
        }

        public virtual Task<bool> CanNavigateAsync(NavigationParameters parameters)
        {
            return Task.FromResult(true);
        }

        protected BusyIndicatorSection StartBusyIndicator()
        {
            if (this.indicatorSection == null)
            {
                this.indicatorSection = new BusyIndicatorSection(this);
            }

            return this.indicatorSection;
        }
    }
}