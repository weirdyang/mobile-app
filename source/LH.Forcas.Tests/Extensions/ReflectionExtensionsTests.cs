using LH.Forcas.Extensions;
using NUnit.Framework;

namespace LH.Forcas.Tests.Extensions
{
    [TestFixture]
    public class ReflectionExtensionsTests
    {
        public class WhenGettingSiblingResourceName : ReflectionExtensionsTests
        {
            [Test]
            public void ThenShouldReturnCorrectName()
            {
                var actual = this.GetType().GetSiblingResourceName("dummy");
                Assert.AreEqual("LH.Forcas.Tests.Extensions.dummy", actual);
            }
        }

        public class WhenGettingResourceContentAsText : ReflectionExtensionsTests
        {
            [Test]
            public void ThenShouldReturnCorrectContent()
            {
                var resourceName = $"{this.GetType().Namespace}.TestResource.txt";
                var content = this.GetType().Assembly.GetManifestResourceContentAsText(resourceName);

                Assert.AreEqual("Dummy Resource Content", content);
            }
        }
    }
}