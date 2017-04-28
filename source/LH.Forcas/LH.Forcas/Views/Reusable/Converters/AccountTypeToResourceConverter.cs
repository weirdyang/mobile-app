namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Localization;
    using Xamarin.Forms;

    public class AccountTypeToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var accountType = (Type) value;
            var accountTypeName = accountType.Name.Substring(0, accountType.Name.Length - 7);

            return AppResources.ResourceManager.GetString($"AccountType_{accountTypeName}", App.CurrentCultureInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}