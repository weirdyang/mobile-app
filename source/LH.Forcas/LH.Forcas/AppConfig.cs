using System;
using LH.Forcas.Contract;

namespace LH.Forcas
{
    public class AppConfig : IAppConfig
    {
        public string ConfigDataGitHubUrl => "https://github.com/lholota/LH.Forcas.Sync";

        public TimeSpan ConfigDataMaxAge => TimeSpan.FromHours(4);

        public int ConfigDataRetryCount => 3;
    }
}