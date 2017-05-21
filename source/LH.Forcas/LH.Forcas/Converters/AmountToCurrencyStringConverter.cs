using System;
using System.Globalization;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Services;
using MvvmCross.Platform.Converters;

namespace LH.Forcas.Converters
{
    public class AmountToCurrencyStringConverter : MvxValueConverter<Amount, string>
    {
        public IRefDataService RefDataService { get; set; }

        protected override string Convert(Amount value, Type targetType, object parameter, CultureInfo culture)
        {
            var currency = this.RefDataService.GetCurrency(value.CurrencyId);
            return string.Format(culture, currency.DisplayFormat, value.Value);
        }
    }
}