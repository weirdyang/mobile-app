using System;
using System.Globalization;
using Xamarin.Forms;

namespace LH.Forcas.Views.Reusable.Converters
{
    public class StringToHtmlWebViewSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valueAsString = value as string;

            if (valueAsString == null)
            {
                return null;
            }

            var result = new HtmlWebViewSource();
            result.Html = valueAsString;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}