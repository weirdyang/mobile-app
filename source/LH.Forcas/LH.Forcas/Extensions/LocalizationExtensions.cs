namespace LH.Forcas.Extensions
{
    using Domain.RefData;
    using Localization;

    public static class LocalizationExtensions
    {
        public static string ToLocalizedResourceString(this object value, string keyFormat = null)
        {
            if (value == null)
            {
                return null;
            }

            string resxKey = $"{value}";

            if (!string.IsNullOrEmpty(keyFormat))
            {
                resxKey = string.Format(keyFormat, value);
            }

            return AppResources.ResourceManager.GetString(resxKey, App.CurrentCultureInfo);
        }

        public static string ToCountryDisplayName(this Country country)
        {
            return ToLocalizedResourceString(country.CountryId, "Country_{0}");
        }

        public static string ToCurrencyDisplayName(this Currency currency)
        {
            return ToLocalizedResourceString(currency.CurrencyId, "Currency_{0}");
        }
    }
}