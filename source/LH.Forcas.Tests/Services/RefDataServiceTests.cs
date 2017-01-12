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
            
            this.refDataService = new RefDataService(
                this.crashReporterMock.Object,
                this.repositoryMock.Object);
        }

        private RefDataService refDataService;
        private Mock<ICrashReporter> crashReporterMock;
        private Mock<IRefDataRepository> repositoryMock;

        [Test]
        public async Task ShouldServeSubsequentFetchFromCache()
        {
            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });

            await this.refDataService.GetCountriesAsync();
            await this.refDataService.GetCountriesAsync();

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
    }
}