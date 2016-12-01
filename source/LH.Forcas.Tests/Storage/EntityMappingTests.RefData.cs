using AutoMapper;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Entities.RefData;
using NUnit.Framework;
using TestStack.Dossier;

namespace LH.Forcas.Tests.Storage
{
    public class EntityMappingRefDataTests
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [Test]
        public void ShouldMapBankD2E()
        {
            var domain = Builder<Bank>.CreateNew().Build();
            var entity = Mapper.Instance.Map<BankEntity>(domain);

            Assert.AreEqual(domain.Name, entity.Name);
            Assert.AreEqual(domain.IbanFormat, entity.IbanFormat);
            Assert.AreEqual(domain.CountryCode, entity.CountryCode);
            Assert.AreEqual(domain.RoutingCode, entity.RoutingCode);
        }

        [Test]
        public void ShouldMapBankE2D()
        {
            var entity = Builder<BankEntity>.CreateNew().Build();
            var domain = Mapper.Instance.Map<Bank>(entity);

            Assert.AreEqual(entity.Name, domain.Name);
            Assert.AreEqual(entity.IbanFormat, domain.IbanFormat);
            Assert.AreEqual(entity.CountryCode, domain.CountryCode);
            Assert.AreEqual(entity.RoutingCode, domain.RoutingCode);
        }

        [Test]
        public void ShouldMapCountryD2E()
        {
            var domain = Builder<Country>.CreateNew().Build();
            var entity = Mapper.Instance.Map<CountryEntity>(domain);

            Assert.AreEqual(domain.Code, entity.Code);
            Assert.AreEqual(domain.DefaultCurrencyCode, entity.DefaultCurrencyCode);
        }

        [Test]
        public void ShouldMapCountryE2D()
        {
            var entity = Builder<CountryEntity>.CreateNew().Build();
            var domain = Mapper.Instance.Map<Country>(entity);

            Assert.AreEqual(entity.Code, domain.Code);
            Assert.AreEqual(entity.DefaultCurrencyCode, domain.DefaultCurrencyCode);
        }

        [Test]
        public void ShouldMapCurrencyD2E()
        {
            var domain = Builder<Currency>.CreateNew().Build();
            var entity = Mapper.Instance.Map<CurrencyEntity>(domain);

            Assert.AreEqual(domain.Symbol, entity.Symbol);
            Assert.AreEqual(domain.ShortCode, entity.ShortCode);
            Assert.AreEqual(domain.DisplayName, entity.DisplayName);
            Assert.AreEqual((short)domain.PreferedSymbolPosition, entity.PreferedSymbolPosition);
        }

        [Test]
        public void ShouldMapCurrencyE2D()
        {
            var entity = Builder<CurrencyEntity>.CreateNew().Build();
            var domain = Mapper.Instance.Map<Currency>(entity);

            Assert.AreEqual(entity.Symbol, domain.Symbol);
            Assert.AreEqual(entity.ShortCode, domain.ShortCode);
            Assert.AreEqual(entity.DisplayName, domain.DisplayName);
            Assert.AreEqual((PrefferedCcySymbolLocation)entity.PreferedSymbolPosition, domain.PreferedSymbolPosition);
        }
    }
}
