using System;

namespace LH.Forcas.Contract
{
    public interface IAppConfig
    {
        string ConfigDataGitHubUrl { get; }

        TimeSpan ConfigDataMaxAge { get; }

        int ConfigDataRetryCount { get; }
    }
}