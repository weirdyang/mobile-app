using System;
using LH.Forcas.Contract;
using LH.Forcas.Integration;
using Xamarin.Forms;

[assembly:Dependency(typeof(DummyCrashReporter))]

namespace LH.Forcas.Integration
{
    public class DummyCrashReporter : ICrashReporter
    {
        public void ReportException(Exception ex)
        {
        }

        public void ReportFatal(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
