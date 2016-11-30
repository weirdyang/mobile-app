using System;

namespace LH.Forcas
{
    public interface IAppConstants
    {
        string ConfigDataGitHubRepoUrl { get; }

        TimeSpan ConfigDataMaxAge { get; }

        int HttpRequestRetryCount { get; }

        int HttpRequestRetryWaitTimeBase { get; }
    }
}