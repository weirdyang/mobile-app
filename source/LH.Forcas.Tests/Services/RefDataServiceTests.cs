using System;
using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Models.RefData;
using LH.Forcas.Services;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Services
{
    [TestFixture]
    public class RefDataServiceTests
    {
        [SetUp]
        public void Setup()
        {
            this.crashReporterMock = new Mock<ICrashReporter>();
            this.repositoryMock = new Mock<IRefDataRepository>(MockBehavior.Strict);
            this.downloaderMock = new Mock<IRefDataDownloader>();
            
            this.dependencyService = new TestsDependencyService();
            this.dependencyService.Register(this.crashReporterMock.Object);
            this.dependencyService.Register(this.repositoryMock.Object);
            this.dependencyService.Register(this.downloaderMock.Object);

            this.refDataService = new RefDataService(this.dependencyService);
        }

        private RefDataService refDataService;
        private TestsDependencyService dependencyService;
        private Mock<ICrashReporter> crashReporterMock;
        private Mock<IRefDataDownloader> downloaderMock;
        private Mock<IRefDataRepository> repositoryMock;

        [Test]
        public async Task ShouldServeSubsequenceFetchFromCache()
        {
            this.repositoryMock.Setup(x => x.GetRefDataAsync<Country>()).ReturnsAsync(new[] { new Country() });

            await this.refDataService.GetRefDataAsync<Country>();
            await this.refDataService.GetRefDataAsync<Country>();

            this.repositoryMock.Verify(x => x.GetRefDataAsync<Country>(), Times.Once);
        }

        [Test]
        public async Task UpdateShouldInvalidateCache()
        {
            var updates = new RefDataUpdateBase[1];

            this.repositoryMock.Setup(x => x.GetRefDataAsync<Country>()).ReturnsAsync(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync(updates);
            this.repositoryMock.Setup(x => x.SaveRefDataUpdates(updates)).Returns(Task.FromResult(0));

            await this.refDataService.GetRefDataAsync<Country>();
            await this.refDataService.UpdateRefDataAsync();
            await this.refDataService.GetRefDataAsync<Country>();

            this.repositoryMock.Verify(x => x.GetRefDataAsync<Country>(), Times.Exactly(2));
            this.repositoryMock.Verify(x => x.SaveRefDataUpdates(updates), Times.Once);
        }

        [Test]
        public async Task UpdateWithNoResultShouldNotInvalidateCache()
        {
            this.repositoryMock.Setup(x => x.GetRefDataAsync<Country>()).ReturnsAsync(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync((RefDataUpdateBase[])null);

            await this.refDataService.GetRefDataAsync<Country>();
            await this.refDataService.UpdateRefDataAsync();
            await this.refDataService.GetRefDataAsync<Country>();

            this.repositoryMock.Verify(x => x.GetRefDataAsync<Country>(), Times.Once);
        }

        [Test]
        public async Task UpdateWithNoResultShouldNotBeSaved()
        {
            this.repositoryMock.Setup(x => x.GetRefDataAsync<Country>()).ReturnsAsync(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync((RefDataUpdateBase[])null);

            await this.refDataService.GetRefDataAsync<Country>();
            await this.refDataService.UpdateRefDataAsync();

            this.repositoryMock.Verify(x => x.GetRefDataAsync<Country>(), Times.Once);
        }

        [Test]
        public void ShouldReportFatalCrashWhenFetchFails()
        {
            var ex = new Exception("Dummy");

            this.repositoryMock.Setup(x => x.GetRefDataAsync<Country>()).ThrowsAsync(ex);
            this.crashReporterMock.Setup(x => x.ReportFatal(ex));

            Assert.ThrowsAsync<Exception>(async () => await this.refDataService.GetRefDataAsync<Country>());

            this.crashReporterMock.Verify(x => x.ReportFatal(ex), Times.Once);
        }

        [Test]
        public async Task ShouldReportErrorsOccuringDuringUpdate()
        {
            var ex = new Exception("Dummy");

            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ThrowsAsync(ex);
            this.crashReporterMock.Setup(x => x.ReportException(ex));

            await this.refDataService.UpdateRefDataAsync();
            
            this.crashReporterMock.Verify(x => x.ReportException(ex), Times.Once);
        }
    }
}