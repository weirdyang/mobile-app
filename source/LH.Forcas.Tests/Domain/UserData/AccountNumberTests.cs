namespace LH.Forcas.Tests.Domain.UserData
{
    using System;
    using Forcas.Domain.UserData;
    using NUnit.Framework;

    public abstract class AccountNumberTests
    {
        [TestFixture]
        public class ParseTests
        {
            [Test]
            public void ShouldThrowIfInputIsEmpty()
            {
                Assert.Catch<ArgumentException>(() => AccountNumber.Parse(null));
            }

            [Test]
            public void ShouldThrowIfFormatDoesNotMatch()
            {
                Assert.Catch<ArgumentException>(() => AccountNumber.Parse("sdfsdfgsd456fg46sd"));
            }

            [Test]
            public void ShouldParseFullNumber()
            {
                var result = AccountNumber.Parse("123-567/5500");

                Assert.AreEqual(123, result.Prefix);
                Assert.AreEqual(567, result.Number);
                Assert.AreEqual(5500, result.BankRoutingCode);
            }

            [Test]
            public void ShouldParseNumberWithoutPrefix()
            {
                var result = AccountNumber.Parse("567/5500");

                Assert.AreEqual(567, result.Number);
                Assert.AreEqual(5500, result.BankRoutingCode);
            }
        }

        [TestFixture]
        public class EqualityTests
        {
            private AccountNumber numberA = new AccountNumber {Prefix = 123, Number = 567, BankRoutingCode = 5500};
            private AccountNumber numberAClone = new AccountNumber {Prefix = 123, Number = 567, BankRoutingCode = 5500};
            private AccountNumber numberB = new AccountNumber {Prefix = 123, Number = 567, BankRoutingCode = 5501};

            [Test]
            public void EqualsShouldReturnTrueForSameValues()
            {
                Assert.IsTrue(this.numberA.Equals(this.numberAClone));
            }

            [Test]
            public void EqualsShouldReturnFalseForDifferentValues()
            {
                Assert.IsFalse(this.numberA.Equals(this.numberB));
            }

            [Test]
            public void GetHashCodeShouldReturnSameCodesForSameValues()
            {
                Assert.AreEqual(this.numberA.GetHashCode(), this.numberAClone.GetHashCode());
            }

            [Test]
            public void GetHashCodeShouldReturnDifferentCodesForDifferentValues()
            {
                Assert.AreNotEqual(this.numberA.GetHashCode(), this.numberB.GetHashCode());
            }

            [Test]
            public void EqualOperatorShouldReturnTrueForSameValues()
            {
                Assert.IsTrue(this.numberA == this.numberAClone);
            }

            [Test]
            public void EqualOperatorShouldReturnFalseForDifferentValues()
            {
                Assert.IsFalse(this.numberA == this.numberB);
            }

            [Test]
            public void InequalOperatorShouldReturnTrueForDifferentValues()
            {
                Assert.IsTrue(this.numberA != this.numberB);
            }

            [Test]
            public void InequalOperatorShouldReturnFalseForSameValues()
            {
                Assert.IsFalse(this.numberA != this.numberAClone);
            }
        }

        [TestFixture]
        public class ToStringTests
        {
            [Test]
            public void NumberWithoutPrefixShouldRenderWithoutDash()
            {
                var number = new AccountNumber();
                number.BankRoutingCode = 5500;
                number.Number = 123;

                Assert.AreEqual("123/5500", number.ToString());
            }

            [Test]
            public void NumberWithPrefixShouldRenderInFull()
            {
                var number = new AccountNumber();
                number.BankRoutingCode = 5500;
                number.Number = 123;
                number.Prefix = 567;

                Assert.AreEqual("567-123/5500", number.ToString());
            }
        }
    }
}