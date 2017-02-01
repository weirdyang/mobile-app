namespace LH.Forcas.Views.Reusable.Controls
{
    using Xamarin.Forms;

    [ContentProperty(nameof(ContentTemplate))]
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(ContentControl));

        public ContentControl()
        {
            this.BindingContextChanged += (sender, e) => this.UpdateContent();
        }

        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ContentTemplateProperty);
            }
            set
            {
                this.SetValue(ContentTemplateProperty, value);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(this.ContentTemplate))
            {
                this.UpdateContent();
            }
        }

        private void UpdateContent()
        {
            if (this.BindingContext == null || this.ContentTemplate == null)
            {
                this.Content = null;
                return;
            }

            var template = this.ContentTemplate;
            var selector = template as DataTemplateSelector;
            if (selector != null)
            {
                template = selector.SelectTemplate(this.BindingContext, this);
            }

            var content = (View)template.CreateContent();
            this.Content = content;
        }
    }
}
