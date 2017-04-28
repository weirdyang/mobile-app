using Octokit;

namespace LH.Forcas.Sync.RefData
{
    public interface IGitHubClientFactory
    {
        IGitHubClient CreateClient();
    }
}