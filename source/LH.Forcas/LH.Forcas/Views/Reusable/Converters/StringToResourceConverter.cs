namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Extensions;
    using Xamarin.Forms;

    public class StringToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string resxFormat = (string) parameter;

            return value.ToLocalizedResourceString(resxFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Should not be used");
        }
    }
}