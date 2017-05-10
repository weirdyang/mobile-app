using System.Threading;

namespace LH.Forcas.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Prism.Mvvm;
    using Prism.Navigation;

    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        private bool isBusy;
        private Task currentBackgroundTask;
        private readonly object currentBackgroundTaskLock = new object();

        public bool IsBusy
        {
            get { return this.isBusy; }
            private set { this.SetProperty(ref this.isBusy, value); }
        }

        public Task CurrentBackgroundTask
        {
            get
            {
                lock (this.currentBackgroundTaskLock)
                {
                    return this.currentBackgroundTask;
                }
            }
            private set
            {
                lock (this.currentBackgroundTaskLock)
                {
                    this.currentBackgroundTask = value;
                }
            }
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters) {}

        public virtual void OnNavigatedTo(NavigationParameters parameters) {}

        protected Task RunAsyncWithBusyIndicator(Action action)
        {
            return this.RunAsyncWithBusyIndicatorImpl(() => Task.Run(action));
        }

        protected Task RunAsyncWithBusyIndicator(Func<Task> asyncCall)
        {
            return this.RunAsyncWithBusyIndicatorImpl(asyncCall);
        }

        private Task RunAsyncWithBusyIndicatorImpl(Func<Task> innerAsyncCall)
        {
            this.IsBusy = true;

            var result = Task.Run(async () =>
            {
                try
                {
                    await innerAsyncCall.Invoke();
                }
                finally
                {
                    this.IsBusy = false;
                }
            });

            this.CurrentBackgroundTask = result;

            return result;
        }
    }
}