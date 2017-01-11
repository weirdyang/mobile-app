using LH.Forcas.iOS.Localization;
using Xamarin.Forms;

[assembly: Dependency(typeof(IosLocale))]

namespace LH.Forcas.iOS.Localization
{
    using System.Globalization;
    using Forcas.Localization;
    using Foundation;

    class IosLocale : ILocale
    {
        public CultureInfo GetCultureInfo()
        {
            var netLanguage = "en-US";

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var pref = NSLocale.PreferredLanguages[0];

                netLanguage = pref.Replace("_", "-");

                // -- Handling unsupported langauge codes --
                switch (netLanguage)
                {
                    case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                    case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                        netLanguage = "ms"; // closest supported
                        break;
                    case "gsw":
                    case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                        netLanguage = "de-CH"; // closest supported
                        break;

                    case "pt":
                        netLanguage = "pt-PT";
                        break;
                    
                        // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
                }
            }

            return new CultureInfo(netLanguage);
        }
    }
}
