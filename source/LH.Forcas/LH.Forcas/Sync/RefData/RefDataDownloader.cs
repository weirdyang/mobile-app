using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.RefData;
using Newtonsoft.Json;
using Octokit;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataDownloader : IRefDataDownloader
    {
        public const string OwnerName = "lholota";
        public const string RepositoryName = "LH.Forcas.Sync";

        private const string JsonSeparator = "//---------------";

        private readonly IApp app;
        private readonly IGitHubClientFactory clientFactory;
        private readonly IAnalyticsReporter analyticsReporter;

        public RefDataDownloader(IApp app, IGitHubClientFactory clientFactory, IAnalyticsReporter analyticsReporter)
        {
            this.analyticsReporter = analyticsReporter;
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

                if (!this.TryParseJson(contents[0].Content, out RefDataUpdate update, out int dataVersion))
                {
                    return new RefDataDownloadResult(newIncompatibleDataAvailable: true);
                }

                return new RefDataDownloadResult(update, contents[0].Sha, dataVersion);
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

        private bool TryParseJson(string json, out RefDataUpdate update, out int dataVersion)
        {
            var parts = json.Split(new[] { JsonSeparator }, StringSplitOptions.None);

            if (parts.Length != 2)
            {
                throw new ArgumentException($"The downloaded json must contain UpdateInfo and UpdateData separated by '{JsonSeparator}'", nameof(json));
            }

            try
            {
                var versionInfo = JsonConvert.DeserializeObject<RefDataUpdateInfo>(parts[0]);
                dataVersion = versionInfo.Version;

                if (this.app.AppVersion < versionInfo.MinAppVersion)
                {
                    update = null;
                    return false;
                }

                update = JsonConvert.DeserializeObject<RefDataUpdate>(parts[1]);
                return true;
            }
            catch (JsonException ex)
            {
                throw new Exception("Deserializing the Json file failed.", ex);
            }
        }
    }
}