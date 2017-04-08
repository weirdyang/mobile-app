using Octokit;

namespace LH.Forcas.Sync.RefData
{
    public class GitHubClientFactory : IGitHubClientFactory
    {
        public const string GitHubProductInfo = "Forcas";

        public IGitHubClient CreateClient()
        {
            return new GitHubClient(new ProductHeaderValue(GitHubProductInfo)); // TODO: Add version from assembly info
        }
    }
}
