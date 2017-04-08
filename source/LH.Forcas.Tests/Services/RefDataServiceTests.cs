using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Analytics;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using LH.Forcas.Sync.RefData;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Services
{
    using System.Linq;

    [TestFixture]
    public class RefDataServiceTests
    {
        protected RefDataService RefDataService;
        protected Mock<IRefDataRepository> RepositoryMock;
        protected Mock<IRefDataDownloader> DownloaderMock;
        protected Mock<IAnalyticsReporter> AnalyticsReporterMock;

        [SetUp]
        public void Setup()
        {
            this.RepositoryMock = new Mock<IRefDataRepository>(MockBehavior.Strict);
            this.DownloaderMock = new Mock<IRefDataDownloader>();
            this.AnalyticsReporterMock = new Mock<IAnalyticsReporter>();

            this.RefDataService = new RefDataService(
                this.RepositoryMock.Object, 
                this.DownloaderMock.Object,
                this.AnalyticsReporterMock.Object);
        }

        public class WhenLoadingData : RefDataServiceTests
        {
            [Test]
            public void ShouldFilterOutInactiveData()
            {
                this.RepositoryMock.Setup(x => x.GetCountries())
                    .Returns(new[]
                    {
                        new Country { IsActive = true, CountryId = "CZ" },
                        new Country { IsActive = false, CountryId = "GB" }
                    });

                var countries = this.RefDataService.GetCountries();

                Assert.AreEqual(1, countries.Count);
                Assert.AreEqual("CZ", countries.Single().CountryId);
            }

            [Test]
            public void ShouldServeSubsequentFetchFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });

                this.RefDataService.GetCountries();
                this.RefDataService.GetCountries();

                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Once);
            }
        }

        public class WhenUpdatingData : RefDataServiceTests
        {
            [Test]
            public async Task ShouldNotSaveWhenUpdateNotAvailable()
            {
                var updateStatus = new RefDataStatus();
                var downloaderResult = new RefDataDownloadResult();

                this.RepositoryMock
                    .Setup(x => x.GetStatus())
                    .Returns(updateStatus);

                this.DownloaderMock
                    .Setup(x => x.DownloadRefData(It.IsAny<RefDataStatus>()))
                    .ReturnsAsync(downloaderResult);

                await this.RefDataService.UpdateRefData();

                this.RepositoryMock.VerifyAll();
            }

            [Test]
            public async Task ShouldSaveRefDataAndStatusWhenUpdateAvailable()
            {
                var updateStatus = new RefDataStatus("sha1", 1);

                var update = new RefDataUpdate
                {
                    Banks = new List<Bank>
                    {
                        new Bank {BankId = "B1", LastChangedVersion = 2}
                    }
                };

                var downloaderResult = new RefDataDownloadResult(update, "sha2", 2);

                this.RepositoryMock.Setup(x => x.GetStatus()).Returns(updateStatus);
                this.RepositoryMock
                    .Setup(x => x.SaveRefDataUpdate(update,It.Is<RefDataStatus>(s => s.CommitSha == "sha2" && s.DataVersion == 2)));

                this.DownloaderMock
                    .Setup(x => x.DownloadRefData(It.IsAny<RefDataStatus>()))
                    .ReturnsAsync(downloaderResult);

                await this.RefDataService.UpdateRefData();

                this.RepositoryMock.VerifyAll();
            }

            [Test]
            public async Task ShouldNotSaveUnchangedEntities()
            {
                var updateStatus = new RefDataStatus("sha1", 1);

                var update = new RefDataUpdate
                {
                    Banks = new List<Bank>
                    {
                        new Bank {BankId = "B1", LastChangedVersion = 2},
                        new Bank {BankId = "B2", LastChangedVersion = 1}
                    }
                };

                var downloaderResult = new RefDataDownloadResult(update, "sha2", 2);

                this.RepositoryMock.Setup(x => x.GetStatus()).Returns(updateStatus);
                this.RepositoryMock
                    .Setup(x => x.SaveRefDataUpdate(
                        It.Is<RefDataUpdate>(u => u.Banks.Count() == 1),
                        It.Is<RefDataStatus>(s => s.CommitSha == "sha2" && s.DataVersion == 2)));

                this.DownloaderMock
                    .Setup(x => x.DownloadRefData(It.IsAny<RefDataStatus>()))
                    .ReturnsAsync(downloaderResult);

                await this.RefDataService.UpdateRefData();

                this.RepositoryMock.VerifyAll();
            }
        }
    }
}