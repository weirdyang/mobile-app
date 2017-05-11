using System;

namespace LH.Forcas
{
    public class AppConfig : IAppConfig
    {
        public string ConfigDataGitHubRepoUrl => "https://github.com/repos/lh-forcas/sync/";

        public TimeSpan ConfigDataMaxAge => TimeSpan.FromHours(4);

        public int HttpRequestRetryCount => 3;

        public int HttpRequestRetryWaitTimeBase => 2;
    }
}