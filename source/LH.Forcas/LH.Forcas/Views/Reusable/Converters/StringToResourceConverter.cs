namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Localization;
    using Xamarin.Forms;

    public class StringToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string resxKey = $"{value}";

            var format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                resxKey = string.Format(format, value);
            }

            return AppResources.ResourceManager.GetString(resxKey, App.CurrentCultureInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Should not be used");
        }
    }
}