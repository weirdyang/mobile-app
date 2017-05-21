using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace LH.Forcas.Sync.RefData
{
    public class GitHubClientFactory : IGitHubClientFactory
    {
        public const string GitHubProductName = "Forcas";

        private readonly IApp app;

        public GitHubClientFactory(IApp app)
        {
            this.app = app;
        }

        public IGitHubClient CreateClient()
        {
            return new GitHubClient(new ProductHeaderValue(GitHubProductName, this.app.AppVersion.ToString()));
        }
    }
}
