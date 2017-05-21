using System.Collections.Generic;
using System.Dynamic;
using LH.Forcas.Extensions;
using NUnit.Framework;

namespace LH.Forcas.Tests.Extensions
{
    [TestFixture]
    public class DynamicExtensionsTests
    {
        public class WhenTryingToGetProperty : DynamicExtensionsTests
        {
            [Test]
            public void ThenShouldReturnValidPropertyValue()
            {
                var expando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>) expando;
                
                dictionary.Add("MyProp", "Value");

                string actual;
                Assert.True(expando.TryGetPropertyValue("MyProp", out actual));
                Assert.AreEqual("Value", actual);
            }

            [Test]
            public void ThenShouldReturnReturnFalseOnInvalidProperty()
            {
                var expando = new ExpandoObject();

                string actual;
                Assert.False(expando.TryGetPropertyValue("NotExisting", out actual));
                Assert.Null(actual);
            }
        }
    }
}