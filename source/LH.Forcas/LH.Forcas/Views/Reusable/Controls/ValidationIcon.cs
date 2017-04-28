namespace LH.Forcas.Views.Reusable.Controls
{
    using FluentValidation.Results;
    using FormsPlugin.Iconize;
    using Xamarin.Forms;

    public class ValidationIcon : IconLabel
    {
        public static readonly BindableProperty ValidationResultProperty = 
            BindableProperty.Create(
                "ValidationResult", 
                typeof(ValidationResult), 
                typeof(ValidationIcon), 
                null,
                propertyChanged:HandlePropertyChanged);

        private static void HandlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var icon = (ValidationIcon)bindable;
            var newResult = (ValidationResult) newvalue;

            if (newResult == null)
            {
                icon.Text = null;
            }
            else if (newResult.IsValid)
            {
                icon.TextColor = Color.Green;
                icon.Text = "md-check-circle";
            }
            else
            {
                icon.TextColor = Color.Red;
                icon.Text = "md-error-outline";
            }
        }

        public ValidationResult ValidationResult
        {
            get { return (ValidationResult) this.GetValue(ValidationResultProperty); }
            set { this.SetValue(ValidationResultProperty, value); }
        }
    }
}