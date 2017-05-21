using System;
using System.Globalization;
using LH.Forcas.Domain.UserData;
using MvvmCross.Platform.Converters;

namespace LH.Forcas.Converters
{
    public class AmountToColorConverter : MvxValueConverter<Amount, string>
    {
        public string PositiveColor { get; set; }

        public string NegativeColor { get; set; }

        protected override string Convert(Amount value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Value >= 0 ? this.PositiveColor : this.NegativeColor;
        }
    }
}