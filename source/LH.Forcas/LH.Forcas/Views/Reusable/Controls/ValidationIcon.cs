namespace LH.Forcas.Views.Reusable.Controls
{
    using FormsPlugin.Iconize;
    using ViewModels;
    using Xamarin.Forms;

    public class ValidationIcon : IconImage
    {
        public static readonly BindableProperty ValidationStateProperty = 
            BindableProperty.Create(
                "ValidationState", 
                typeof(ValidationState), 
                typeof(ValidationIcon), 
                false,
                propertyChanged:HandlePropertyChanged);

        private static void HandlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var icon = (ValidationIcon) bindable;

            if (icon.ValidationState == null)
            {
                icon.Icon = null;
            }
            else if (icon.ValidationState.IsValid)
            {
                icon.IconColor = Color.Green;
                icon.Icon = "md-check-circle";
            }
            else
            {
                icon.IconColor = Color.Red;
                icon.Icon = "md-error-outline";
            }
        }

        public ValidationState ValidationState
        {
            get { return (ValidationState) this.GetValue(ValidationStateProperty); }
            set { this.SetValue(ValidationStateProperty, value); }
        }
    }
}