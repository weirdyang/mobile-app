using System;
using System.Threading.Tasks;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Integration;
using LH.Forcas.Services;
using LH.Forcas.Storage;
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
            
            this.refDataService = new RefDataService(
                this.crashReporterMock.Object,
                this.downloaderMock.Object,
                this.repositoryMock.Object);
        }

        private RefDataService refDataService;
        private Mock<ICrashReporter> crashReporterMock;
        private Mock<IRefDataDownloader> downloaderMock;
        private Mock<IRefDataRepository> repositoryMock;

        [Test]
        public async Task ShouldServeSubsequenceFetchFromCache()
        {
            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });

            await this.refDataService.GetCountriesAsync();
            await this.refDataService.GetCountriesAsync();

            this.repositoryMock.Verify(x => x.GetCountries(), Times.Once);
        }

        [Test]
        public async Task UpdateShouldInvalidateCache()
        {
            var updates = new IRefDataUpdate[1];

            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync(updates);
            this.repositoryMock.Setup(x => x.SaveRefDataUpdates(updates));

            await this.refDataService.GetCountriesAsync();
            await this.refDataService.UpdateRefDataAsync();
            await this.refDataService.GetCountriesAsync();

            this.repositoryMock.Verify(x => x.GetCountries(), Times.Exactly(2));
            this.repositoryMock.Verify(x => x.SaveRefDataUpdates(updates), Times.Once);
        }

        [Test]
        public async Task UpdateWithNoResultShouldNotInvalidateCache()
        {
            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync((IRefDataUpdate[])null);

            await this.refDataService.GetCountriesAsync();
            await this.refDataService.UpdateRefDataAsync();
            await this.refDataService.GetCountriesAsync();

            this.repositoryMock.Verify(x => x.GetCountries(), Times.Once);
        }

        [Test]
        public async Task UpdateWithNoResultShouldNotBeSaved()
        {
            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });
            this.downloaderMock.Setup(x => x.GetRefDataUpdates(It.IsAny<DateTime>())).ReturnsAsync((IRefDataUpdate[])null);

            await this.refDataService.GetCountriesAsync();
            await this.refDataService.UpdateRefDataAsync();

            this.repositoryMock.Verify(x => x.GetCountries(), Times.Once);
        }

        [Test]
        public void ShouldReportFatalCrashWhenFetchFails()
        {
            var ex = new Exception("Dummy");

            this.repositoryMock.Setup(x => x.GetCountries()).Throws(ex);
            this.crashReporterMock.Setup(x => x.ReportFatal(ex));

            Assert.ThrowsAsync<Exception>(async () => await this.refDataService.GetCountriesAsync());

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