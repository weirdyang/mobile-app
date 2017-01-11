namespace LH.Forcas.Converters
{
    using System;
    using System.Globalization;
    using Localization;
    using Xamarin.Forms;
    public class EnumToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var enumType = value.GetType();
            var resxKey = $"{enumType}Enum_{value}";

            return AppResources.ResourceManager.GetString(resxKey, App.CurrentCultureInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Should not be used");
        }
    }
}