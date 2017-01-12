namespace LH.Forcas.Views.Reusable.Behaviours
{
    using System;
    using ViewModels;
    using Xamarin.Forms;

    public class ValidationBehavior : Behavior<Entry>
    {
        static readonly BindablePropertyKey ValidationStatePropertyKey = 
            BindableProperty.CreateReadOnly("ValidationState", typeof(bool), typeof(ValidationBehavior), false);

        public static readonly BindableProperty ValidationStateProperty = ValidationStatePropertyKey.BindableProperty;

        private IValidated validatedViewModel;
        private string propertyName;

        public ValidationState ValidationState
        {
            get { return (ValidationState)this.GetValue(ValidationStateProperty); }
            private set { this.SetValue(ValidationStateProperty, value); }
        }

        public string PropertyName
        {
            get { return this.propertyName; }
            set
            {
                this.propertyName = value;
                this.OnPropertyChanged();
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += this.HandleTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= this.HandleTextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var validated = this.BindingContext as IValidated;
            if (validated != null)
            {
                this.validatedViewModel = validated;
            }
            else
            {
                this.validatedViewModel = null;
            }
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.validatedViewModel == null)
            {
#if DEBUG
                throw new Exception($"The viewmodel {this.BindingContext} does not support validation.");
#endif

                return;
            }

            if (string.IsNullOrEmpty(this.PropertyName))
            {
#if DEBUG
                throw new Exception($"The property name should not be empty.");
#endif
                return;
            }

            this.ValidationState = this.validatedViewModel.Validate(this.PropertyName);
        }
    }
}