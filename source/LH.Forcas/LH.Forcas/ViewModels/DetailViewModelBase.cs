namespace LH.Forcas.ViewModels
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Internal;
    using Localization;
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Services;

    public abstract class DetailViewModelBase : ViewModelBase, IConfirmNavigationAsync
    {
        protected readonly IPageDialogService DialogService;

        protected IValidator Validator;

        protected DetailViewModelBase(IPageDialogService dialogService)
        {
            this.DialogService = dialogService;

            this.SaveCommand = this.CreateAsyncCommand(this.Save, this.CanSave);
        }

        public bool IsDirty { get; protected set; }

        public ValidationResults ValidationResults { get; private set; }

        public DelegateCommand SaveCommand { get; }

        public async Task<bool> CanNavigateAsync(NavigationParameters parameters)
        {
            if (this.IsDirty)
            {
                var dialogResult = await this.DialogService.DisplayAlertAsync(
                    AppResources.ConfirmDirtyDialog_Title,
                    AppResources.ConfirmDirtyDialog_Description,
                    AppResources.ConfirmDialog_Yes,
                    AppResources.ConfirmDialog_No);

                return dialogResult;
            }

            return true;
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            var changed = base.SetProperty(ref storage, value, propertyName);

            if (propertyName != nameof(this.IsBusy) && propertyName != nameof(this.ValidationResults))
            {
                if (changed && this.Validator != null)
                {
                    this.RefreshValidation(propertyName);
                }

                if (changed)
                {
                    this.SaveCommand.RaiseCanExecuteChanged();

                    this.IsDirty = true;
                    this.RaisePropertyChanged(nameof(this.IsDirty));
                }
            }

            // ReSharper restore ExplicitCallerInfoArgument
            return changed;
        }

        protected virtual Task Save()
        {
            this.IsDirty = false;

            return Task.FromResult(0);
        }

        protected virtual bool CanSave()
        {
            if (this.Validator != null)
            {
                this.RefreshValidation();
                return this.ValidationResults.IsValid;
            }

            return true;
        }

        private void RefreshValidation(string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                var fluentResults = this.Validator.Validate(this);
                this.ValidationResults = new ValidationResults(fluentResults);
            }
            else
            {
                var propertiesToValidate = new[] {propertyName};
                var ctx = new ValidationContext(this, new PropertyChain(), new MemberNameValidatorSelector(propertiesToValidate));

                var fluentResult = this.Validator.Validate(ctx);

                var validationResults = this.ValidationResults ?? new ValidationResults();
                validationResults.UpdateProperty(propertyName, fluentResult);

                this.ValidationResults = validationResults;
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            this.RaisePropertyChanged(nameof(this.ValidationResults));
        }
    }
}