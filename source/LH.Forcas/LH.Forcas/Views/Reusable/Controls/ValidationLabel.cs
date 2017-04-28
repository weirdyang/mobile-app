namespace LH.Forcas.Views.Reusable.Controls
{
    using System.Linq;
    using FluentValidation.Results;
    using Xamarin.Forms;

    public class ValidationLabel : Label
    {
        public static readonly BindableProperty ValidationResultProperty =
         BindableProperty.Create(
             "ValidationResult",
             typeof(ValidationResult),
             typeof(ValidationLabel),
             null,
             propertyChanged: HandlePropertyChanged);

        public static readonly BindableProperty InfoMessageProperty =
         BindableProperty.Create(
             "InfoMessageProperty",
             typeof(string),
             typeof(ValidationLabel));

        private static Color? _originalTextColor;

        private static void HandlePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var label = (ValidationLabel)bindable;
            var newResult = (ValidationResult)newvalue;

            if (!_originalTextColor.HasValue)
            {
                _originalTextColor = label.TextColor;
            }

            if (newResult == null || newResult.IsValid)
            {
                label.TextColor = _originalTextColor.Value;
                label.Text = label.InfoMessage;
            }
            else
            {
                label.TextColor = Color.Red;
                label.Text = string.Join(" ", newResult.Errors.Select(x => x.ErrorMessage));
            }
        }

        public string InfoMessage
        {
            get { return (string)this.GetValue(InfoMessageProperty); }
            set { this.SetValue(InfoMessageProperty, value); }
        }

        public ValidationResult ValidationResult
        {
            get { return (ValidationResult)this.GetValue(ValidationResultProperty); }
            set { this.SetValue(ValidationResultProperty, value); }
        }
    }
}