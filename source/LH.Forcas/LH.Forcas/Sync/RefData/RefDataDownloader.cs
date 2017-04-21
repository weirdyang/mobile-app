using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.RefData;
using LH.Forcas.RefDataContract.Parsing;
using Octokit;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataDownloader : IRefDataDownloader
    {
        public const string OwnerName = "lholota";
        public const string RepositoryName = "LH.Forcas.Sync";

        private readonly IApp app;
        private readonly IRefDataUpdateParser updateParser;
        private readonly IGitHubClientFactory clientFactory;
        private readonly IAnalyticsReporter analyticsReporter;

        public RefDataDownloader(IApp app, IGitHubClientFactory clientFactory, IAnalyticsReporter analyticsReporter, IRefDataUpdateParser updateParser)
        {
            this.analyticsReporter = analyticsReporter;
            this.updateParser = updateParser;
            this.clientFactory = clientFactory;
            this.app = app;
        }

        private string BranchName
        {
            get
            {
#if DEBUG
                return "dev";
#else
                return "master";
#endif
            }
        }

        public async Task<RefDataDownloadResult> DownloadRefData(RefDataStatus status)
        {
            if (status == null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            try
            {
                var client = this.clientFactory.CreateClient();

                if (status != null)
                {
                    var branch = await client.Repository.Branch.Get(OwnerName, RepositoryName, this.BranchName);

                    if (string.Equals(branch.Commit.Sha, status.CommitSha, StringComparison.OrdinalIgnoreCase))
                    {
                        return new RefDataDownloadResult();
                    }
                }

                var filePath = $"{this.BranchName}/Data.json";
                var contents = await client.Repository.Content.GetAllContents(OwnerName, RepositoryName, filePath);

                var parserResult = this.updateParser.Parse(contents[0].Content, this.app.AppVersion, status.DataVersion);
                if (parserResult == null)
                {
                    return new RefDataDownloadResult();
                }

                return new RefDataDownloadResult(parserResult.Update, contents[0].Sha, parserResult.DataVersion);
            }
            catch (ApiException ex)
            {
                Debug.WriteLine($"GitHub Octokit has thrown an exception: {ex}");
            }
            catch (Exception ex)
            {
                this.analyticsReporter.ReportHandledException(ex, "RefData download failed.");
            }

            return new RefDataDownloadResult();
        }
    }
}