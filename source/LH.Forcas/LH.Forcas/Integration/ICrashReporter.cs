using System;

namespace LH.Forcas.Integration
{
    public interface ICrashReporter
    {
        void ReportException(Exception ex);

        void ReportFatal(Exception ex);
    }
}
