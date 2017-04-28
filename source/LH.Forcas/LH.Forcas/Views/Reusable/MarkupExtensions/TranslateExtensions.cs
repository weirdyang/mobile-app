namespace LH.Forcas.Views.Reusable.MarkupExtensions
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty(nameof(Key))]
    public class TranslateExtension : IMarkupExtension
    {
        const string ResourceId = "LH.Forcas.Localization.AppResources";

        public string Key { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Key == null)
            {
                return string.Empty;
            }

            var temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = temp.GetString(this.Key, App.CurrentCultureInfo);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{this.Key}' was not found in resources '{ResourceId}' for culture '{App.CurrentCultureInfo}'.",
                    nameof(this.Key));
#else
				translation = Key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }
    }
}
