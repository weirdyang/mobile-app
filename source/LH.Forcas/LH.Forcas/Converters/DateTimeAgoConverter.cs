using System;
using System.Globalization;
using LH.Forcas.Localization;
using MvvmCross.Platform.Converters;

namespace LH.Forcas.Converters
{
    public class DateTimeAgoConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan timeSpan;

            if (value is TimeSpan)
            {
                timeSpan = (TimeSpan) value;
            }
            else if(value is DateTime)
            {
                var date = (DateTime) value;
                timeSpan = DateTime.UtcNow - date;
            }
            else
            {
                return null;
            }

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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Should not be used");
        }

        private bool IsInRange(double total, int unit, string multipleUnitsMessageFormat, string singleUnitMessage, out string message)
        {
            message = null;

            if (total >= unit*2)
            {
                message = string.Format(multipleUnitsMessageFormat, Math.Floor(total/unit));
            }
            else if(total >= unit)
            {
                message = singleUnitMessage;
            }

            return !string.IsNullOrEmpty(message);
        }
    }
}
