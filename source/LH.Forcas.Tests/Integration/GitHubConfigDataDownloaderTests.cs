using System;
using Flurl.Http.Testing;
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

            appConfigMock.SetupGet(x => x.ConfigDataGitHubUrl).Returns("https://github.com/lholota/LH.Forcas.IntegrationTests");
            appConfigMock.SetupGet(x => x.ConfigDataMaxAge).Returns(TimeSpan.FromMilliseconds(1));
            appConfigMock.SetupGet(x => x.ConfigDataRetryCount).Returns(3);

            this.crashReporter = new Mock<ICrashReporter>();
            this.downloader = new GitHubConfigDataDownloader(appConfigMock.Object, crashReporter.Object);
            this.flurlTest = new HttpTest();
        }

        [TearDown]
        public void TearDown()
        {
            this.flurlTest.Dispose();
        }

        private HttpTest flurlTest;
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
        public void ShouldRetryOnTimeout()
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