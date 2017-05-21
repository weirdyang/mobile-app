using NUnit.Framework;

namespace LH.Forcas.Tests.Sync.RefData
{
    using System;
    using Forcas.Sync.RefData;
    using Moq;
    using Octokit;

    [TestFixture]
    public class GitHubClientFactoryTests
    {
        protected static readonly Version ExpectedVersion = Version.Parse("1.2.3.4");

        protected GitHubClientFactory Factory;
        protected Mock<IApp> AppMock;

        [SetUp]
        public void Setup()
        {
            this.AppMock = new Mock<IApp>();
            this.AppMock.SetupGet(x => x.AppVersion).Returns(ExpectedVersion);

            this.Factory = new GitHubClientFactory(this.AppMock.Object);
        }

        public class WhenCreatingClient : GitHubClientFactoryTests
        {
            [Test]
            public void ShouldPassAppNameAndVersion()
            {
                var client = this.Factory.CreateClient();

                var connection = (Connection)client.Connection;

                AssertEx.Contains(ExpectedVersion.ToString(), connection.UserAgent);
                AssertEx.Contains(GitHubClientFactory.GitHubProductName, connection.UserAgent);
            }
        }
    }
}