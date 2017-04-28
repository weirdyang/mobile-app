using System;

namespace LH.Forcas.Views.Reusable.Converters
{
    using System.Globalization;
    using Xamarin.Forms;
    public class BankIdToImageSourceConverter : IValueConverter
    {
        private const string ResourceNameFormat = "LH.Forcas.Views.Reusable.Images.Banks.{0}-lg.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bankId = value as string;
            if (string.IsNullOrEmpty(bankId))
            {
                return null;
            }

            var resourceName = string.Format(ResourceNameFormat, bankId);
            return ImageSource.FromResource(resourceName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}