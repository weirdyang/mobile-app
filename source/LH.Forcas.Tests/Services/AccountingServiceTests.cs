using System.Collections.Generic;
using LH.Forcas.Domain.UserData;
using LH.Forcas.Services;
using LH.Forcas.Storage;
using Moq;
using NUnit.Framework;

namespace LH.Forcas.Tests.Services
{
    [TestFixture]
    public class AccountingServiceTests
    {
        protected AccountingService Service;
        protected Mock<IUserDataRepository> UserDataRepositoryMock;

        [SetUp]
        public void Setup()
        {
            this.UserDataRepositoryMock = new Mock<IUserDataRepository>();
            this.Service = new AccountingService(this.UserDataRepositoryMock.Object);
        }

        public class WhenHandlingAccounts : AccountingServiceTests
        {
            [Test]
            public void ShouldReturnAccountsFromRepository()
            {
                var expected = new List<Account>
                {
                    new CashAccount(),
                    new CheckingAccount()
                };

                this.UserDataRepositoryMock.Setup(x => x.GetAccounts()).Returns(expected);

                var actual = this.Service.GetAccounts();

                Assert.AreEqual(expected, actual);
            }
        }
    }
}