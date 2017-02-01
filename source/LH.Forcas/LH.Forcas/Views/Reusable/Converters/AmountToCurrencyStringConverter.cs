namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Domain.UserData;
    using Services;
    using Xamarin.Forms;

    public class AmountToCurrencyStringConverter : IValueConverter
    {
        public IRefDataService RefDataService { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Amount))
            {
                return null;
            }

            var amount = (Amount)value;

            if (string.IsNullOrEmpty(amount.CurrencyId) || amount.Value == default(decimal))
            {
                throw new ArgumentException("The amount must have valid values for all properties to convert.");
            }

            var currency = this.RefDataService.GetCurrency(amount.CurrencyId);
            return string.Format(currency.DisplayFormat, amount.Value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}