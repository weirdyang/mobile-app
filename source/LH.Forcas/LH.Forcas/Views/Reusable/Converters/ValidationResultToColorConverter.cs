namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using FluentValidation.Results;
    using Xamarin.Forms;

    public class ValidationResultToColorConverter : IValueConverter
    {
        public ValidationResultToColorConverter()
        {
            this.HighlightValid = false;
        }

        public bool HighlightValid { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var validationResult = value as ValidationResult;

            if (validationResult != null)
            {
                if (validationResult.IsValid && this.HighlightValid)
                {
                    return Color.Green;
                }

                if (!validationResult.IsValid)
                {
                    return Color.Red;
                }
            }

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used.");
        }
    }
}