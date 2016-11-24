using System;
using LH.Forcas.Contract;
using Polly;
using Polly.Retry;

namespace LH.Forcas.Extensions
{
    public static class PollyExtensions
    {
        public static RetryPolicy WaitAndRetryExponentialAsync(this PolicyBuilder policyBuilder, IApp app)
        {
            Func<int, TimeSpan> timeCalcFunction = attempt => TimeSpan.FromSeconds(Math.Pow(app.Constants.HttpRequestRetryWaitTimeBase, attempt));

            return policyBuilder.WaitAndRetryAsync(app.Constants.HttpRequestRetryCount, timeCalcFunction);
        }
    }
}