using System;
using Polly;
using Polly.Retry;

namespace LH.Forcas.Extensions
{
    public static class PollyExtensions
    {
        public static RetryPolicy WaitAndRetryExponentialAsync(this PolicyBuilder policyBuilder, IAppConfig appConfig)
        {
            Func<int, TimeSpan> timeCalcFunction = attempt => TimeSpan.FromSeconds(Math.Pow(appConfig.HttpRequestRetryWaitTimeBase, attempt));

            return policyBuilder.WaitAndRetryAsync(appConfig.HttpRequestRetryCount, timeCalcFunction);
        }
    }
}