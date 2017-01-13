namespace LH.Forcas.ViewModels
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using Prism.Mvvm;
    using Prism.Navigation;

    public abstract class ViewModelBase : BindableBase, IConfirmNavigationAsync
    {
        private bool isBusy;
        private BusyIndicatorSection indicatorSection;

        protected IValidator Validator;

        protected ViewModelBase()
        {
            this.ValidationResults = new ValidationResults();
        }

        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        public ValidationResults ValidationResults { get; }

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
            else
            {
                this.indicatorSection.PushNested();
            }

            return this.indicatorSection;
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