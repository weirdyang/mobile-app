using System;
using System.Threading.Tasks;
using Prism.Commands;

namespace LH.Forcas.ViewModels
{
    public class AsyncDelegateCommand : DelegateCommand
    {
        private readonly Action executeMethod;

        public AsyncDelegateCommand(ViewModelBase viewModel, Action executeMethod)
            : base(() => viewModel.RunAsyncWithBusyIndicator(executeMethod))
        {
            this.executeMethod = executeMethod;
        }

        public AsyncDelegateCommand(ViewModelBase viewModel, Action executeMethod, Func<bool> canExecuteMethod)
            : base(() => viewModel.RunAsyncWithBusyIndicator(executeMethod), canExecuteMethod)
        {
            this.executeMethod = executeMethod;
        }

        public AsyncDelegateCommand(ViewModelBase viewModel, Func<Task> asyncCall)
            : base(() => viewModel.RunAsyncWithBusyIndicator(asyncCall))
        {
            this.executeMethod = () => asyncCall.Invoke().Wait();
        }

        public AsyncDelegateCommand(ViewModelBase viewModel, Func<Task> asyncCall, Func<bool> canExecuteMethod)
            : base(() => viewModel.RunAsyncWithBusyIndicator(asyncCall), canExecuteMethod)
        {
            this.executeMethod = () => asyncCall.Invoke().Wait();
        }

        public void ExecuteSync()
        {
            this.executeMethod.Invoke();
        }
    }

    public class AsyncDelegateCommand<T> : DelegateCommand<T>
    {
        private readonly Func<T, Task> asyncCall;

        public AsyncDelegateCommand(ViewModelBase viewModel, Func<T, Task> asyncCall)
            : base(param => viewModel.RunAsyncWithBusyIndicator(asyncCall, param))
        {
            this.asyncCall = asyncCall;
        }

        public AsyncDelegateCommand(ViewModelBase viewModel, Func<T, Task> asyncCall, Func<T, bool> canExecuteMethod)
            : base(param => viewModel.RunAsyncWithBusyIndicator(asyncCall, param), canExecuteMethod)
        {
            this.asyncCall = asyncCall;
        }

        public void ExecuteSync(T param)
        {
            this.asyncCall.Invoke(param).Wait();
        }
    }
}