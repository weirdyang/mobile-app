using System;
using LH.Forcas.Contract;
using LH.Forcas.Integration;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Integration
{
    [TestFixture]
    public class GitHubConfigDataDownloaderTests
    {
        [SetUp]
        public void Setup()
        {
            var appConfigMock = new Mock<IAppConfig>();
            appConfigMock
                .SetupGet(x => x.ConfigDataGitHubUrl)
                .Returns("https://github.com/lholota/LH.Forcas.IntegrationTests");

            appConfigMock
                .SetupGet(x => x.ConfigDataMaxAge)
                .Returns(TimeSpan.FromMilliseconds(1));

            this.crashReporter = new Mock<ICrashReporter>();
            this.downloader = new GitHubConfigDataDownloader(appConfigMock.Object, crashReporter.Object);
        }

        private Mock<ICrashReporter> crashReporter;
        private GitHubConfigDataDownloader downloader;

        [Test]
        public void SyncWithNoLastSyncShouldReturnLatest()
        {
            
            throw new NotImplementedException();
        }

        [Test]
        public void SyncWithLastSyncShouldReturnDiff()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldRetryOnError()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldReportIfSyncFailingForTooLong()
        {
            throw new NotImplementedException();
        }
    }
}