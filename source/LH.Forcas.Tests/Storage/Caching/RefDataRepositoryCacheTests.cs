using System;
using System.Collections.Generic;
using LH.Forcas.Domain.RefData;
using LH.Forcas.Events;
using LH.Forcas.RefDataContract;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Caching;
using Moq;
using MvvmCross.Plugins.Messenger;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage.Caching
{
    [TestFixture]
    public class RefDataRepositoryCacheTests
    {
        private Action<TrimMemoryRequestedEvent> trimMemoryEventCallback;

        protected RefDataRepositoryCache Cache;
        protected Mock<IRefDataRepository> RepositoryMock;
        protected Mock<IMvxMessenger> MessengerMock;

        [SetUp]
        public void Setup()
        {
            this.RepositoryMock = new Mock<IRefDataRepository>();

            this.MessengerMock = new Mock<IMvxMessenger>();
            this.MessengerMock.SetupMessengerSubscribe<TrimMemoryRequestedEvent>(action => this.trimMemoryEventCallback = action);

            this.Cache = new RefDataRepositoryCache(
                this.RepositoryMock.Object,
                this.MessengerMock.Object);
        }

        protected void PublishTrimMemoryEvent(TrimMemorySeverity severity)
        {
            var evt = new TrimMemoryRequestedEvent(this)
            {
                Severity = severity
            };
            
            this.trimMemoryEventCallback.Invoke(evt);
        }

        public class WhenHandlingBanks : RefDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldHandleSubsequentCallsFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetBanks()).Returns(new List<Bank>());

                this.Cache.GetBanks();
                this.Cache.GetBanks();

                this.RepositoryMock.Verify(x => x.GetBanks(), Times.Once);
            }

            [Test]
            public void ThenRefDataUpdateContainingBanksShouldInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Banks = new List<Bank>
                    {
                        new Bank()
                    }
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetBanks()).Returns(new List<Bank>());

                this.Cache.GetBanks();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetBanks();

                this.RepositoryMock.Verify(x => x.GetBanks(), Times.Exactly(2));
            }

            [Test]
            public void ThenRefDataUpdateNotContainingBanksShouldNotInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Banks = new List<Bank>()
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetBanks()).Returns(new List<Bank>());

                this.Cache.GetBanks();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetBanks();

                this.RepositoryMock.Verify(x => x.GetBanks(), Times.Exactly(1));
            }
        }

        public class WhenHandlingCountries : RefDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldHandleSubsequentCallsFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new List<Country>());

                this.Cache.GetCountries();
                this.Cache.GetCountries();

                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Once);
            }

            [Test]
            public void ThenRefDataUpdateContainingCountriesShouldInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Countries = new List<Country>
                    {
                        new Country()
                    }
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new List<Country>());

                this.Cache.GetCountries();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetCountries();

                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Exactly(2));
            }

            [Test]
            public void ThenRefDataUpdateNotContainingCountriesShouldNotInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Countries = new List<Country>()
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new List<Country>());

                this.Cache.GetCountries();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetCountries();

                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Exactly(1));
            }
        }

        public class WhenHandlingCurrencies : RefDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldHandleSubsequentCallsFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetCurrencies()).Returns(new List<Currency>());

                this.Cache.GetCurrencies();
                this.Cache.GetCurrencies();

                this.RepositoryMock.Verify(x => x.GetCurrencies(), Times.Once);
            }

            [Test]
            public void ThenRefDataUpdateContainingCurrenciesShouldInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Currencies = new List<Currency>
                    {
                        new Currency()
                    }
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetCurrencies()).Returns(new List<Currency>());

                this.Cache.GetCurrencies();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetCurrencies();

                this.RepositoryMock.Verify(x => x.GetCurrencies(), Times.Exactly(2));
            }

            [Test]
            public void ThenRefDataUpdateNotContainingCountriesShouldNotInvalidateCache()
            {
                var update = new RefDataUpdate
                {
                    Currencies = new List<Currency>()
                };

                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetCurrencies()).Returns(new List<Currency>());

                this.Cache.GetCurrencies();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetCurrencies();

                this.RepositoryMock.Verify(x => x.GetCurrencies(), Times.Exactly(1));
            }
        }

        public class WhenHandlingStatus : RefDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldHandleSubsequentCallsFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetStatus()).Returns(new RefDataStatus());

                this.Cache.GetStatus();
                this.Cache.GetStatus();

                this.RepositoryMock.Verify(x => x.GetStatus(), Times.Once);
            }

            [Test]
            public void ThenRefDataUpdateShouldInvalidateCache()
            {
                var update = new RefDataUpdate();
                var status = new RefDataStatus();

                this.RepositoryMock.Setup(x => x.GetStatus()).Returns(new RefDataStatus());
                this.RepositoryMock.Setup(x => x.SaveRefDataUpdate(It.IsAny<RefDataUpdate>(), It.IsAny<RefDataStatus>()));

                this.Cache.GetStatus();
                this.Cache.SaveRefDataUpdate(update, status);
                this.Cache.GetStatus();

                this.RepositoryMock.Verify(x => x.GetStatus(), Times.Exactly(2));
            }
        }

        public class WhenTrimmingMemory : RefDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldTrimAllOnCompleteTrimRequest()
            {
                this.RepositoryMock.Setup(x => x.GetBanks()).Returns(new List<Bank>());
                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new List<Country>());
                this.RepositoryMock.Setup(x => x.GetCurrencies()).Returns(new List<Currency>());

                this.Cache.GetBanks();
                this.Cache.GetCountries();
                this.Cache.GetCurrencies();

                this.PublishTrimMemoryEvent(TrimMemorySeverity.ReleaseAll);

                this.Cache.GetBanks();
                this.Cache.GetCountries();
                this.Cache.GetCurrencies();

                this.RepositoryMock.Verify(x => x.GetBanks(), Times.Exactly(2));
                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Exactly(2));
                this.RepositoryMock.Verify(x => x.GetCurrencies(), Times.Exactly(2));
            }

            [Test]
            public void ThenShouldTrimBanksOnReleaseLevelRequest()
            {
                this.RepositoryMock.Setup(x => x.GetBanks()).Returns(new List<Bank>());
                this.RepositoryMock.Setup(x => x.GetCountries()).Returns(new List<Country>());
                this.RepositoryMock.Setup(x => x.GetCurrencies()).Returns(new List<Currency>());

                this.Cache.GetBanks();
                this.Cache.GetCountries();
                this.Cache.GetCurrencies();

                this.PublishTrimMemoryEvent(TrimMemorySeverity.ReleaseLevel);

                this.Cache.GetBanks();
                this.Cache.GetCountries();
                this.Cache.GetCurrencies();

                this.RepositoryMock.Verify(x => x.GetBanks(), Times.Exactly(2));
                this.RepositoryMock.Verify(x => x.GetCountries(), Times.Once);
                this.RepositoryMock.Verify(x => x.GetCurrencies(), Times.Once);
            }
        }
    }
}