using System;
using System.Diagnostics;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace LH.Forcas.ViewModels
{
    public class ActivityIndicatorState : MvxNotifyPropertyChanged
    {
        private bool isBusy;

        public bool IsBusy
        {
            get
            {
                Debug.WriteLine("Getting IsBusy ({0})", this.isBusy);
                return this.isBusy;
            }
            set
            {
                Debug.WriteLine("Setting IsBusy ({0})", this.isBusy);
                this.SetProperty(ref this.isBusy, value);
            }
        }

        public async Task RunWithIndicator(Action action)
        {
            var asyncCall = this.WrapWithIndicator(action);
            await asyncCall();
        }

        public Func<Task> WrapWithIndicator(Action syncCall)
        {
            return async () =>
            {
                try
                {
                    this.IsBusy = true;
                    await Task.Run(syncCall);
                }
                finally
                {
                    this.IsBusy = false;
                }
            };
        }

        public Func<Task> WrapWithIndicator(Func<Task> asyncCall)
        {
            return async () =>
            {
                try
                {
                    this.IsBusy = true;
                    await asyncCall.Invoke();
                }
                finally
                {
                    this.IsBusy = false;
                }
            };
        }

        public Func<TParam, Task> WrapWithIndicator<TParam>(Func<TParam, Task> asyncCall)
        {
            return async param =>
            {
                try
                {
                    this.IsBusy = true;
                    await asyncCall.Invoke(param);
                }
                finally
                {
                    this.IsBusy = false;
                }
            };
        }
    }
}