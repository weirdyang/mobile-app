using LH.Forcas.Droid.Localization;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidLocale))]

namespace LH.Forcas.Droid.Localization
{
    using System.Globalization;
    using Forcas.Localization;

    public class DroidLocale : ILocale
    {
        public CultureInfo GetCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var dotNetLocale = androidLocale.ToString().Replace("_", "-");
            
            return new CultureInfo(dotNetLocale);
        }
    }
}