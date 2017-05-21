using System;
using System.Globalization;
using System.Reflection;
using LH.Forcas.Localization;
using MvvmCross.Platform.Converters;

namespace LH.Forcas.Converters
{
    public class ToResourceConverter : IMvxValueConverter
    {
        public string ResxPrefix { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string resxKey = null;
            var valueTypeInfo = value.GetType().GetTypeInfo();

            if (valueTypeInfo.IsEnum)
            {
                resxKey = $"{valueTypeInfo.Name}Enum_{value}";
            }
            else if(value is Type)
            {
                resxKey = $"{this.ResxPrefix}_{((Type)value).Name}";
            }

            return AppResources.ResourceManager.GetString(resxKey, App.CurrentCultureInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }
    }
}