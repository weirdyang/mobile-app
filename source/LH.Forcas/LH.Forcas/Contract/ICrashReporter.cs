using System;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface ICrashReporter
    {
        void ReportException(Exception ex);

        void ReportFatal(Exception ex);
    }
}
