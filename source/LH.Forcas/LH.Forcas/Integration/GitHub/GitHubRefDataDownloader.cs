using System;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Extensions;
using LH.Forcas.Integration.Exceptions;
using LH.Forcas.Integration.GitHub;
using Polly;
using Xamarin.Forms;

[assembly:Dependency(typeof(GitHubRefDataDownloader))]

namespace LH.Forcas.Integration.GitHub
{
    public class GitHubRefDataDownloader : IRefDataDownloader
    {
        private readonly IAppConfig appConfig;

        public GitHubRefDataDownloader(IAppConfig appConfig)
        {
            this.appConfig = appConfig;
        }

        public async Task<IRefDataUpdate[]> GetRefDataUpdates(DateTime? lastSyncTime)
        {
            var result = new IRefDataUpdate[3];
            var updatesAvailable = await this.AreUpdatesAvailableAsync(lastSyncTime);

            if (!updatesAvailable)
            {
                return null;
            }

            result[0] = await this.FetchRefDataFileAsync<Country>();
            result[1] = await this.FetchRefDataFileAsync<Currency>();
            result[2] = await this.FetchRefDataFileAsync<Bank>();

            return result;
        }

        private async Task<bool> AreUpdatesAvailableAsync(DateTime? lastSyncTime)
        {
            var commits = await this.ExecuteWithRetry(async () =>
                    await this.appConfig.ConfigDataGitHubRepoUrl
                        .AppendPathSegment("commits")
                        .SetQueryParams(new { since = lastSyncTime?.ToString("o") })
                        .GetJsonListAsync()
            );

            return commits != null && commits.Any();
        }

        private async Task<IRefDataUpdate> FetchRefDataFileAsync<T>()
        {
            var uri = this.appConfig.ConfigDataGitHubRepoUrl.AppendPathSegment($"{typeof(T).Name}.json");

            ExpandoObject fileInfo = await this.ExecuteWithRetry(
                async () => await uri.GetJsonAsync()
            );

            if (fileInfo == null)
            {
                throw new RefDataFormatException($"The request to {uri} returned no json response.");
            }

            string jsonBase64String;
            if (!fileInfo.TryGetPropertyValue("content", out jsonBase64String))
            {
                throw new RefDataFormatException($"The response from {uri} does not contain the 'content' property.");
            }

            try
            {
                var jsonBytes = Convert.FromBase64String(jsonBase64String);
                var jsonString = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<RefDataUpdate<T>>(jsonString);
            }

            catch (Exception ex)
            {
                throw new RefDataFormatException("Parsing of the response from GitHub has failed.", ex);
            }
        }

        private async Task<T> ExecuteWithRetry<T>(Func<Task<T>> fetchCall)
        {
            return await Policy
                .Handle<HttpRequestException>()
                .Or<FlurlHttpException>()
                .WaitAndRetryExponentialAsync(this.appConfig)
                .ExecuteAsync(fetchCall.Invoke);
        }
    }
}