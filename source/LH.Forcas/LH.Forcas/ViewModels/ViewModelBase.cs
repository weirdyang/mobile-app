using System;
using System.Threading.Tasks;
using Prism.Commands;
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

        protected DelegateCommand CreateAsyncCommand(Func<Task> asyncCall, Func<bool> canExecute = null)
        {
            // How to pass the parameter and make it strongly typed?
            Action wrappedAction = () => this.RunAsyncWithBusyIndicator(asyncCall);

            if (canExecute == null)
            {
                return new DelegateCommand(wrappedAction);
            }

            return new DelegateCommand(wrappedAction, canExecute);
        }

        protected DelegateCommand CreateAsyncCommand(Action call, Func<bool> canExecute = null)
        {
            // How to pass the parameter and make it strongly typed?
            Action wrappedAction = () => this.RunAsyncWithBusyIndicator(call);

            if (canExecute == null)
            {
                return new DelegateCommand(wrappedAction);
            }

            return new DelegateCommand(wrappedAction, canExecute);
        }

        protected DelegateCommand<T> CreateAsyncCommand<T>(Func<T, Task> asyncCall, Func<T, bool> canExecute = null)
        {
            // How to pass the parameter and make it strongly typed?
            Action<T> wrappedAction = (T param) => this.RunAsyncWithBusyIndicatorImpl(asyncCall, param);

            if (canExecute == null)
            {
                return new DelegateCommand<T>(param => wrappedAction((T)param));
            }

            return new DelegateCommand<T>(wrappedAction, canExecute);
        }

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

        private Task RunAsyncWithBusyIndicatorImpl<T>(Func<T, Task> innerAsyncCall, T param)
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