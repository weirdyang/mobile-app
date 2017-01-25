namespace LH.Forcas.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Prism.Mvvm;
    using Prism.Navigation;

    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        private bool isBusy;

        public bool IsBusy
        {
            get { return this.isBusy; }
            private set { this.SetProperty(ref this.isBusy, value); }
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

        public virtual void OnNavigatedTo(NavigationParameters parameters) { }

        protected Task RunAsyncWithBusyIndicator(Action action)
        {
            this.IsBusy = true;

            return Task.Run(() =>
                     {
                         try
                         {
                             action.Invoke();
                         }
                         finally
                         {
                             //Device.BeginInvokeOnMainThread(() => { this.IsBusy = false; });
                             this.IsBusy = false;
                         }
                     });
        }

        protected Task RunAsyncWithBusyIndicator(Task task)
        {
            return this.RunAsyncWithBusyIndicator(task.Wait);
        }
    }
}