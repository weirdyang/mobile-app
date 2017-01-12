namespace LH.Forcas.Views.Reusable.Controls
{
    using Xamarin.Forms;
    public class ValidationLabel : Label
    {
        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid", typeof(bool?), typeof(ValidationIcon), false);

        public bool? IsValid
        {
            get { return (bool)this.GetValue(IsValidProperty); }
            set
            {
                this.SetValue(IsValidProperty, value);
            }
        }
    }
}