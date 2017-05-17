using System;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Navigation;

namespace LH.Forcas.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigatingAware
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

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            this.OnNavigatingToAsync(parameters);
        }

        public virtual Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            return Task.FromResult(0);
        }

        public Task RunAsyncWithBusyIndicator(Action action)
        {
            return this.RunAsyncWithBusyIndicator(() => Task.Run(action));
        }

        public Task RunAsyncWithBusyIndicator(Func<Task> asyncCall)
        {
            this.IsBusy = true;

            var result = Task.Run(async () =>
            {
                try
                {
                    await asyncCall.Invoke();
                }
                finally
                {
                    this.IsBusy = false;
                }
            });

            this.CurrentBackgroundTask = result;

            return result;
        }

        public Task RunAsyncWithBusyIndicator<T>(Func<T, Task> innerAsyncCall, T param)
        {
            this.IsBusy = true;

            var result = Task.Run(async () =>
            {
                try
                {
                    await innerAsyncCall.Invoke(param);
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