namespace LH.Forcas.Views.Reusable.Converters
{
    using System;
    using System.Globalization;
    using Localization;
    using Xamarin.Forms;

    public class DateTimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan))
            {
                return null;
            }

            var timeSpan = (TimeSpan) value;
            string result;

            if (this.IsInRange(timeSpan.TotalDays, 365, AppResources.DateTimeAgo_Years, AppResources.DateTimeAgo_Year, out result))
            {
                return result;
            }

            if (this.IsInRange(timeSpan.TotalDays, 30, AppResources.DateTimeAgo_Months, AppResources.DateTimeAgo_Month, out result))
            {
                return result;
            }

            if (this.IsInRange(timeSpan.TotalDays, 7, AppResources.DateTimeAgo_Weeks, AppResources.DateTimeAgo_Week, out result))
            {
                return result;
            }

            if (this.IsInRange(timeSpan.TotalDays, 1, AppResources.DateTimeAgo_Days, AppResources.DateTimeAgo_Day, out result))
            {
                return result;
            }

            if (this.IsInRange(timeSpan.TotalHours, 1, AppResources.DateTimeAgo_Hours, AppResources.DateTimeAgo_Hour, out result))
            {
                return result;
            }

            if (this.IsInRange(timeSpan.TotalMinutes, 1, AppResources.DateTimeAgo_Minutes, AppResources.DateTimeAgo_Minute, out result))
            {
                return result;
            }

            return AppResources.DateTimeAgo_Seconds;
        }

        private bool IsInRange(double total, int unit, string multipleUnitsMessageFormat, string singleUnitMessage, out string message)
        {
            message = null;

            if (total >= unit*2)
            {
                message = string.Format(multipleUnitsMessageFormat, total/unit);
            }
            else if(total >= unit)
            {
                message = singleUnitMessage;
            }

            return !string.IsNullOrEmpty(message);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
