using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Controls
{
    public partial class CardFooter
    {
        public static readonly BindableProperty FooterTextProperty = BindableProperty.Create(
          nameof(FooterText),
          typeof(string),
          typeof(GroupHeader),
          propertyChanged: UpdateLabelText);

        private static void UpdateLabelText(BindableObject bindable, object oldvalue, object newvalue)
        {
            var header = (CardFooter)bindable;
            header.MainLabel.Text = (string)newvalue;
        }

        public CardFooter()
        {
            this.InitializeComponent();
        }

        public string FooterText
        {
            get { return (string)this.GetValue(FooterTextProperty); }
            set { this.SetValue(FooterTextProperty, value); }
        }
    }
}