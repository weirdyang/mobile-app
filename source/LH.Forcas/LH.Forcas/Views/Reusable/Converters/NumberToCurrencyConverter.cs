namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Microsoft.Practices.Unity;
    using Services;
    using Xamarin.Forms;

    public class NumberToCurrencyConverter : IValueConverter
    {
        private readonly IRefDataService refDataService;

        public NumberToCurrencyConverter()
        {
            this.refDataService = App.GlobalContainer.Resolve<IRefDataService>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currencyId = parameter as string;
            if (string.IsNullOrEmpty(currencyId))
            {
                throw new ArgumentNullException(nameof(parameter), "The converter parameter must be a valid currency id.");
            }

            var currency = this.refDataService.GetCurrency(currencyId);
            return string.Format(currency.DisplayFormat, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}