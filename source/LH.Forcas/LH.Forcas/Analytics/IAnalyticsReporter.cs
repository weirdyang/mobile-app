using System;

namespace LH.Forcas.Analytics
{
    public interface IAnalyticsReporter
    {
        void ReportHandledException(Exception ex, string message = null);
    }
}