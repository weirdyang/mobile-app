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

        public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

        public virtual void OnNavigatedTo(NavigationParameters parameters) { }

        protected Task RunAsyncWithBusyIndicator(Action action)
        {
            var task = new Task(action, CancellationToken.None, TaskCreationOptions.None);
            return this.RunAsyncWithBusyIndicator(task);
        }

        protected Task RunAsyncWithBusyIndicator(Func<Task> task)
        {
            return this.RunAsyncWithBusyIndicator(Task.Run(task.Invoke));
        }

        private Task RunAsyncWithBusyIndicator(Task task)
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                return Task.FromResult(0);
            }

            this.IsBusy = true;
            
            task.ContinueWith(x =>
                {
                    this.IsBusy = false;
                    this.CurrentBackgroundTask = null;

                    x.Exception?.Handle(ex => false);
                }, 
                CancellationToken.None,
                TaskContinuationOptions.HideScheduler,
                TaskScheduler.FromCurrentSynchronizationContext());

            this.CurrentBackgroundTask = task;

            task.Start(TaskScheduler.Current);

            return task;
        }
    }
}