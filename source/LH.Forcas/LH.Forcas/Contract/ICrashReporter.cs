using System;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface ICrashReporter
    {
        Task ReportException(Exception ex);

        Task ReportConfigDataNotSyncedForTooLong(DateTime lastSyncTime);
    }
}
