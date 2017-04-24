using LH.Forcas.Domain.UserData;
using LH.Forcas.Events;
using LH.Forcas.Storage;
using LH.Forcas.Storage.Caching;
using Moq;
using NUnit.Framework;
using Prism.Events;

namespace LH.Forcas.Tests.Storage.Caching
{
    [TestFixture]
    public class UserDataRepositoryCacheTests
    {
        protected UserDataRepositoryCache Cache;
        protected Mock<IUserDataRepository> RepositoryMock;
        protected Mock<IEventAggregator> EventAggregatorMock;
        protected TrimMemoryRequestedEvent TrimMemoryEvent;

        [SetUp]
        public void Setup()
        {
            this.RepositoryMock = new Mock<IUserDataRepository>();
            this.EventAggregatorMock = new Mock<IEventAggregator>();
            this.TrimMemoryEvent = new TrimMemoryRequestedEvent();

            this.EventAggregatorMock
                .Setup(x => x.GetEvent<TrimMemoryRequestedEvent>())
                .Returns(this.TrimMemoryEvent);

            this.Cache = new UserDataRepositoryCache(
                this.RepositoryMock.Object,
                this.EventAggregatorMock.Object);
        }

        public class WhenHandlingUserSettings : UserDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldHandleSubsequentCallsFromCache()
            {
                this.RepositoryMock.Setup(x => x.GetUserSettings()).Returns(new UserSettings());

                this.Cache.GetUserSettings();
                this.Cache.GetUserSettings();

                this.RepositoryMock.Verify(x => x.GetUserSettings(), Times.Once);
            }

            [Test]
            public void ThenRefDataUpdateContainingBanksShouldInvalidateCache()
            {
                this.RepositoryMock.Setup(x => x.GetUserSettings()).Returns(new UserSettings());

                this.Cache.GetUserSettings();
                this.Cache.SaveUserSettings(new UserSettings());
                this.Cache.GetUserSettings();

                this.RepositoryMock.Verify(x => x.GetUserSettings(), Times.Exactly(2));
            }
        }

        public class WhenTrimmingMemory : UserDataRepositoryCacheTests
        {
            [Test]
            public void ThenShouldTrimUserSettingsOnReleaseAllRequest()
            {
                this.RepositoryMock.Setup(x => x.GetUserSettings()).Returns(new UserSettings());

                this.Cache.GetUserSettings();

                this.TrimMemoryEvent.Publish(TrimMemorySeverity.ReleaseAll);

                this.Cache.GetUserSettings();

                this.RepositoryMock.Verify(x => x.GetUserSettings(), Times.Exactly(2));
            }
        }
    }
}