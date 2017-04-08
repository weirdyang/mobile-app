using System;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Integration;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Services
{
    using System.Linq;

    [TestFixture]
    public class RefDataServiceTests
    {
        [SetUp]
        public void Setup()
        {
            this.repositoryMock = new Mock<IRefDataRepository>(MockBehavior.Strict);
            
            this.refDataService = new RefDataService(
                this.repositoryMock.Object);
        }

        private RefDataService refDataService;
        private Mock<IRefDataRepository> repositoryMock;

        [Test]
        public void ShouldFilterOutInactiveData()
        {
            this.repositoryMock.Setup(x => x.GetCountries())
                .Returns(new[]
                         {
                             new Country { IsActive = true, CountryId = "CZ" },
                             new Country { IsActive = false, CountryId = "GB" }
                         });

            var countries = this.refDataService.GetCountries();

            Assert.AreEqual(1, countries.Count);
            Assert.AreEqual("CZ", countries.Single().CountryId);
        }

        [Test]
        public void ShouldServeSubsequentFetchFromCache()
        {
            this.repositoryMock.Setup(x => x.GetCountries()).Returns(new[] { new Country() });

            this.refDataService.GetCountries();
            this.refDataService.GetCountries();

            this.repositoryMock.Verify(x => x.GetCountries(), Times.Once);
        }
    }
}