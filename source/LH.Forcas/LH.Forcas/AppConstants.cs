using System;

namespace LH.Forcas
{
    public class AppConstants : IAppConstants
    {
        public string ConfigDataGitHubRepoUrl => "https://github.com/repos/lholota/LH.Forcas.Sync/";

        public TimeSpan ConfigDataMaxAge => TimeSpan.FromHours(4);

        public int HttpRequestRetryCount => 3;

        public int HttpRequestRetryWaitTimeBase => 2;
    }
}