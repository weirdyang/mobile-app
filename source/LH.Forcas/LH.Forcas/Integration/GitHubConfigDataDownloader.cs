using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Contract;

namespace LH.Forcas.Integration
{
    public class GitHubConfigDataDownloader : IConfigDataDownloader
    {
        private readonly IAppConfig appConfig;
        private readonly ICrashReporter crashReporter;

        public GitHubConfigDataDownloader(IAppConfig appConfig, ICrashReporter crashReporter)
        {
            this.appConfig = appConfig;
            this.crashReporter = crashReporter;
        }

        public async Task<Dictionary<string, byte[]>> GetUpdatedFiles()
        {
            throw new System.NotImplementedException();
        }
    }
}