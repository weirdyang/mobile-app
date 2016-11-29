using System;
using LH.Forcas.Contract;
using Polly;
using Polly.Retry;

namespace LH.Forcas.Extensions
{
    public static class PollyExtensions
    {
        public static RetryPolicy WaitAndRetryExponentialAsync(this PolicyBuilder policyBuilder, IAppConstants appConstants)
        {
            Func<int, TimeSpan> timeCalcFunction = attempt => TimeSpan.FromSeconds(Math.Pow(appConstants.HttpRequestRetryWaitTimeBase, attempt));

            return policyBuilder.WaitAndRetryAsync(appConstants.HttpRequestRetryCount, timeCalcFunction);
        }
    }
}