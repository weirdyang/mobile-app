using System;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using LH.Forcas.Contract;
using LH.Forcas.Contract.Exceptions;
using LH.Forcas.Integration;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Integration
{
    [TestFixture]
    public class GitHubRefDataDownloaderTests
    {
        private static readonly string CommitsNoLastSyncResponse = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Commits-NoLastSync.json");
        private static readonly string CommitsOneNewCommitResponse = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Commits-OneNewCommit.json");
        private static readonly string CommitsNoNewCommitResponse = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Commits-NoNewCommit.json");

        private static readonly string ContentValidFile = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Content-ValidFile.json");
        private static readonly string ContentInvalidFile = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Content-InvalidFile.json");
        private static readonly string ContentInvalidResponse = Extensions.LoadFileContents("Integration\\GitHubRefDataResponses\\Content-InvalidResponse.json");

        [SetUp]
        public void Setup()
        {
            this.flurlTest = new HttpTest();

            var appConstantsMock = new Mock<IAppConstants>();
            appConstantsMock.SetupGet(x => x.ConfigDataGitHubRepoUrl).Returns("https://github.com/repos/magic/");
            appConstantsMock.SetupGet(x => x.ConfigDataMaxAge).Returns(TimeSpan.FromMilliseconds(1));
            appConstantsMock.SetupGet(x => x.HttpRequestRetryCount).Returns(3);
            appConstantsMock.SetupGet(x => x.HttpRequestRetryWaitTimeBase).Returns(0);
            
            this.dependencyService = new TestsDependencyService();
            this.dependencyService.Register(appConstantsMock.Object);

            this.downloader = new GitHubRefDataDownloader(this.dependencyService);
        }

        [TearDown]
        public void TearDown()
        {
            this.flurlTest.Dispose();
        }

        private HttpTest flurlTest;
        private GitHubRefDataDownloader downloader;
        private TestsDependencyService dependencyService;

        [Test]
        public async Task ShouldReturnUpdatesWithNoLastSyncDate()
        {
            this.flurlTest.RespondWith(CommitsNoLastSyncResponse);
            this.flurlTest.RespondWith(ContentValidFile);
            this.flurlTest.RespondWith(ContentValidFile);
            this.flurlTest.RespondWith(ContentValidFile);

            var result = await this.downloader.GetRefDataUpdates(null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public async Task ShouldReturnUpdatesWithNewCommits()
        {
            this.flurlTest.RespondWith(CommitsOneNewCommitResponse);
            this.flurlTest.RespondWith(ContentValidFile);
            this.flurlTest.RespondWith(ContentValidFile);
            this.flurlTest.RespondWith(ContentValidFile);

            var result = await this.downloader.GetRefDataUpdates(DateTime.MinValue);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public async Task ShouldReturnNullWhenNoUpdatesAvailable()
        {
            this.flurlTest.RespondWith(CommitsNoNewCommitResponse);

            var result = await this.downloader.GetRefDataUpdates(DateTime.Now);

            Assert.IsNull(result);
        }

        [Test]
        public async Task ShouldRetryOnError()
        {
            this.flurlTest.RespondWith(string.Empty, 429);
            this.flurlTest.RespondWith(string.Empty, 404);
            this.flurlTest.RespondWith(CommitsNoNewCommitResponse);

            var results = await this.downloader.GetRefDataUpdates(DateTime.Now);

            Assert.IsNull(results);
            Assert.AreEqual(3, this.flurlTest.CallLog.Count);
        }

        [Test]
        public async Task ShouldRetryOnTimeout()
        {
            this.flurlTest.SimulateTimeout();
            this.flurlTest.SimulateTimeout();
            this.flurlTest.RespondWith(CommitsNoNewCommitResponse);

            var results = await this.downloader.GetRefDataUpdates(DateTime.Now);

            Assert.IsNull(results);
            Assert.AreEqual(3, this.flurlTest.CallLog.Count);
        }

        [Test]
        public void ShouldThrowWhenResponseIsEmpty()
        {
            this.flurlTest.RespondWith(CommitsOneNewCommitResponse);
            this.flurlTest.RespondWith(string.Empty);

            var exception = Assert.ThrowsAsync<RefDataFormatException>(async () => await this.downloader.GetRefDataUpdates(DateTime.Now));

            Assert.IsTrue(exception.Message.Contains("no json response"));
        }

        [Test]
        public void ShouldThrowWhenContentPropertyIsNotPresent()
        {
            this.flurlTest.RespondWith(CommitsOneNewCommitResponse);
            this.flurlTest.RespondWith(ContentInvalidResponse);

            var exception = Assert.ThrowsAsync<RefDataFormatException>(async () => await this.downloader.GetRefDataUpdates(DateTime.Now));

            Assert.IsTrue(exception.Message.Contains("'content'"));
        }

        [Test]
        public void ShouldThrowWhenParsingFileContentFailed()
        {
            this.flurlTest.RespondWith(CommitsOneNewCommitResponse);
            this.flurlTest.RespondWith(ContentInvalidFile);

            Assert.ThrowsAsync<RefDataFormatException>(async () => await this.downloader.GetRefDataUpdates(DateTime.Now));
        }
    }
}