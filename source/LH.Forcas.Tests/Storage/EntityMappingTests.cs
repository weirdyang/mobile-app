using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Entities.UserData;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    [Ignore("Work in progress")]
    [TestFixture]
    public class EntityMappingUserDataTests
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [Test]
        public void ShouldMapTransactionE2D()
        {
            var entity = new TransactionEntity();
            entity.TransactionType = 1;

            var domain = Mapper.Instance.Map<Transaction>(entity);

            Assert.AreEqual(TransactionType.WireTransfer, domain.TransactionType);
        }

        [Test]
        public void ShouldMapTransactionD2E()
        {
            var domain = new Transaction();
            domain.TransactionType = TransactionType.WireTransfer;

            var entity = Mapper.Instance.Map<TransactionEntity>(domain);

            Assert.AreEqual(1, entity.TransactionType);
        }

        [Test]
        public void ShouldMapBudgetE2D()
        {
            var entity = new BudgetEntity();
            entity.BudgetId = 201405;
            entity.Categories = new List<BudgetCategoryEntity>
            {
                new BudgetCategoryEntity { CategoryId = Guid.NewGuid(), Amount = 10m }
            };

            var domain = Mapper.Instance.Map<Budget>(entity);

            Assert.AreEqual(2014, domain.Year);
            Assert.AreEqual(5, domain.Month);

            Assert.IsNotNull(domain.Categories);
            Assert.AreEqual("Dummy", domain.Categories.Single().CategoryId);
            Assert.AreEqual(10m, domain.Categories.Single().Amount);
        }

        [Test]
        public void ShouldMapBudgetD2E()
        {
            var domain = new Budget();
            domain.Year = 2014;
            domain.Month = 5;

            var entity = Mapper.Instance.Map<BudgetEntity>(domain);

            Assert.AreEqual(201405, entity.BudgetId);
        }
    }
}