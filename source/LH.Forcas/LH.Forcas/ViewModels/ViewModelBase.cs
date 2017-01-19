namespace LH.Forcas.ViewModels
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using Prism.Mvvm;
    using Prism.Navigation;
    using Xamarin.Forms;

    public abstract class ViewModelBase : BindableBase, INavigationAware
    {
        private bool isBusy;

        protected IValidator Validator;

        protected ViewModelBase()
        {
            this.ValidationResults = new ValidationResults();
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            private set { this.SetProperty(ref this.isBusy, value); }
        }

        public ValidationResults ValidationResults { get; }

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

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var changed = base.SetProperty(ref storage, value, propertyName);
            
            if (changed && this.Validator != null && propertyName != "IsBusy")
            {
                var ctx = new ValidationContext(
                    this,
                    new PropertyChain(),
                    new MemberNameValidatorSelector(new [] { propertyName }));

                var results = this.Validator.Validate(ctx);

                this.ValidationResults.PushResults(propertyName, results);
                this.OnPropertyChanged(nameof(this.ValidationResults));
            }

            // ReSharper restore ExplicitCallerInfoArgument
            return changed;
        }
    }
}