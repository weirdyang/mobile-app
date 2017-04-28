namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Domain.UserData;
    using Xamarin.Forms;
    public class AmountToColorConverter : IValueConverter
    {
        public Color PositiveColor { get; set; }

        public Color NegativeColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Amount))
            {
                throw new ArgumentException("Only decimal value conversion is supported.", nameof(value));
            }

            var amount = (Amount) value;

            return amount.Value >= 0 ? this.PositiveColor : this.NegativeColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}