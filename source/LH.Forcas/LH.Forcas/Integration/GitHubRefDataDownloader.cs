using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using LH.Forcas.Contract;
using LH.Forcas.Extensions;
using LH.Forcas.Models.RefData;
using Polly;

namespace LH.Forcas.Integration
{
    public class GitHubRefDataDownloader : IRefDataDownloader
    {
        private readonly IApp app;
        private readonly ICrashReporter crashReporter;

        public GitHubRefDataDownloader(IApp app, ICrashReporter crashReporter)
        {
            this.crashReporter = crashReporter;
            this.app = app;
        }

        public async Task<AppRefData> GetUpdatedFiles()
        {
            var lastSyncTime = GetLastSyncTime();

            try
            {
                AppRefData result = null;
                var updatesAvailable = await this.AreUpdatesAvailableAsync(lastSyncTime);

                if (updatesAvailable)
                {
                    result = new AppRefData();
                    result.Banks = await this.FetchConfigDataFileAsync<Bank[]>();
                }

                this.app.Properties[App.LastRefDataSyncPropertyKey] = DateTime.UtcNow;

                return result;
            }
            catch (Exception ex)
            {
                if (!lastSyncTime.HasValue || DateTime.Now - lastSyncTime.Value > this.app.Constants.ConfigDataMaxAge)
                {
                    this.crashReporter.ReportException(new Exception("Downloading config data has failed.", ex)).Wait();
                }

                return null;
            }
        }

        private DateTime? GetLastSyncTime()
        {
            DateTime? lastSyncTime = null;

            if (this.app.Properties.ContainsKey(App.LastRefDataSyncPropertyKey))
            {
                lastSyncTime = (DateTime) this.app.Properties[App.LastRefDataSyncPropertyKey];
            }

            return lastSyncTime;
        }

        private async Task<bool> AreUpdatesAvailableAsync(DateTime? lastSyncTime)
        {
            var commits = await this.ExecuteWithRetry(async () =>
                    await this.app.Constants.ConfigDataGitHubRepoUrl
                        .AppendPathSegment("commits")
                        .SetQueryParams(new {since = lastSyncTime?.ToString("o")})
                        .GetJsonListAsync()
            );

            return commits != null && commits.Any();
        }

        private async Task<T> FetchConfigDataFileAsync<T>()
        {
            var uri = this.app.Constants.ConfigDataGitHubRepoUrl.AppendPathSegment($"{typeof(T).Name}.json");

            dynamic fileInfo = await this.ExecuteWithRetry(
                async () => await uri.GetJsonAsync()
            );

            try
            {
                if (fileInfo == null)
                {
                    throw new Exception($"The request to {uri} returned no response.");
                }

                var jsonBytes = Convert.FromBase64String((string)fileInfo.content);
                var jsonString = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Parsing of the response from GitHub has failed.", ex);
            }
        }

        private async Task<T> ExecuteWithRetry<T>(Func<Task<T>> fetchCall)
        {
            return await Policy
                .Handle<HttpRequestException>()
                .Or<FlurlHttpException>()
                .WaitAndRetryExponentialAsync(this.app)
                .ExecuteAsync(fetchCall.Invoke);
        }
    }
}