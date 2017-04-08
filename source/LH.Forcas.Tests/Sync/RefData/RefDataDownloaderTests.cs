using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Sync.RefData;
using Moq;
using NUnit.Framework;
using Octokit;

namespace LH.Forcas.Tests.Sync.RefData
{
    [TestFixture]
    public abstract class RefDataDownloaderTests
    {
        public const string ValidJsonFilePath = @"Sync\RefData\ValidRefDataUpdate.json";
        public const string InvalidJsonFilePath = @"Sync\RefData\InvalidRefDataUpdate.json";

        protected RefDataDownloader Downloader;
        protected Mock<IApp> AppMock;
        protected Mock<IGitHubClient> GitHubClientMock;
        protected Mock<IGitHubClientFactory> GitHubClientFactoryMock;
        protected Mock<IAnalyticsReporter> AnalyticsReporterMock;

        protected Mock<IRepositoriesClient> GitHubRepositoriesClientMock;
        protected Mock<IRepositoryBranchesClient> GitHubBranchesClientMock;
        protected Mock<IRepositoryContentsClient> GitHubContentsClientMock;

        [SetUp]
        public void Setup()
        {
            this.AppMock = new Mock<IApp>();
            this.GitHubClientMock = new Mock<IGitHubClient>();
            this.GitHubClientFactoryMock = new Mock<IGitHubClientFactory>();
            this.AnalyticsReporterMock = new Mock<IAnalyticsReporter>(MockBehavior.Strict);

            this.GitHubRepositoriesClientMock = new Mock<IRepositoriesClient>();
            this.GitHubBranchesClientMock = new Mock<IRepositoryBranchesClient>();
            this.GitHubContentsClientMock = new Mock<IRepositoryContentsClient>();

            this.GitHubClientFactoryMock.Setup(x => x.CreateClient()).Returns(this.GitHubClientMock.Object);
            this.GitHubClientMock.SetupGet(x => x.Repository).Returns(this.GitHubRepositoriesClientMock.Object);
            this.GitHubRepositoriesClientMock.SetupGet(x => x.Branch).Returns(this.GitHubBranchesClientMock.Object);
            this.GitHubRepositoriesClientMock.SetupGet(x => x.Content).Returns(this.GitHubContentsClientMock.Object);

            this.Downloader = new RefDataDownloader(
                this.AppMock.Object,
                this.GitHubClientFactoryMock.Object,
                this.AnalyticsReporterMock.Object);
        }

        public class WhenNoUpdatesAvailable : RefDataDownloaderTests
        {
            [Test]
            public async Task ShouldNotDownloadFileContents()
            {
                this.SetupBranchCommit("sha");
                var update = await this.Downloader.DownloadRefData(new RefDataStatus("sha", 1));

                Assert.IsFalse(update.NewDataAvailable);
            }
        }

        public class WhenUpdatesAvailable : RefDataDownloaderTests
        {
            [Test]
            public async Task ShouldReturnUpdate()
            {
                const string newCommitSha = "sha2";

                this.SetupBranchCommit(newCommitSha);
                this.SetupFileContents(ValidJsonFilePath, newCommitSha);
                this.AppMock.SetupGet(x => x.AppVersion).Returns(new Version(3, 0));

                var update = await this.Downloader.DownloadRefData(new RefDataStatus("sha1", 1));

                Assert.IsTrue(update.NewDataAvailable);
                Assert.IsFalse(update.NewIncompatibleDataAvailable);
                Assert.IsNotNull(update.Data);
                Assert.IsNotNull(update.Data.Banks);
                Assert.AreEqual(1, update.Data.Banks.Count());
            }

            [Test]
            public async Task ShouldReturnNullIfUpdateRequiresHigherAppVersion()
            {
                const string newCommitSha = "sha2";

                this.SetupBranchCommit(newCommitSha);
                this.SetupFileContents(ValidJsonFilePath, newCommitSha);
                this.AppMock.SetupGet(x => x.AppVersion).Returns(new Version(1, 0));

                var update = await this.Downloader.DownloadRefData(new RefDataStatus("sha1", 1));

                Assert.IsFalse(update.NewDataAvailable);
                Assert.IsTrue(update.NewIncompatibleDataAvailable);
            }
        }

        public class WhenHandlingFailures : RefDataDownloaderTests
        {
            [Test]
            public async Task ShouldNotReportNetworkFailure()
            {
                var ex = new ApiException("Error", new SocketException());

                this.GitHubBranchesClientMock
                    .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .ThrowsAsync(ex);

                var update = await this.Downloader.DownloadRefData(new RefDataStatus("sha", 1));

                Assert.IsFalse(update.NewDataAvailable);
                Assert.IsFalse(update.NewIncompatibleDataAvailable);
            }

            [Test]
            public async Task ShouldReportInvalidJson()
            {
                const string newCommitSha = "sha2";

                this.SetupBranchCommit(newCommitSha);
                this.SetupFileContents(InvalidJsonFilePath, newCommitSha);

                this.AppMock.SetupGet(x => x.AppVersion).Returns(new Version(3, 0));
                this.AnalyticsReporterMock
                    .Setup(x => x.ReportHandledException(It.IsAny<Exception>(), It.IsAny<string>()));

                var update = await this.Downloader.DownloadRefData(new RefDataStatus("sha1", 1));

                Assert.IsFalse(update.NewDataAvailable);
                Assert.IsFalse(update.NewIncompatibleDataAvailable);

                this.AnalyticsReporterMock.VerifyAll();
            }
        }

        protected void SetupBranchCommit(string commitSha)
        {
            var gitRef = new GitReference(null, null, null, commitSha, new User(), null);
            var branch = new Branch("dev", gitRef, false);

            this.GitHubBranchesClientMock
                .Setup(x => x.Get(RefDataDownloader.OwnerName, RefDataDownloader.RepositoryName, "dev"))
                .ReturnsAsync(branch);
        }

        protected void SetupFileContents(string jsonSourceFilePath, string commitSha)
        {
            var jsonBytes = File.ReadAllBytes(Extensions.GetContentFilePath(jsonSourceFilePath));
            var base64Json = Convert.ToBase64String(jsonBytes);

            var refDataFileContent = new RepositoryContent(null, null, commitSha, 0, ContentType.File, null, null, null, null, "utf8", base64Json, null, null);

            var contents = new List<RepositoryContent>
            {
                refDataFileContent
            };

            this.GitHubContentsClientMock
                .Setup(x => x.GetAllContents(RefDataDownloader.OwnerName, RefDataDownloader.RepositoryName, "dev/Data.json"))
                .ReturnsAsync(contents.AsReadOnly());
        }
    }
}