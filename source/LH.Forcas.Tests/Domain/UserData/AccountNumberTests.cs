namespace LH.Forcas.Tests.Domain.UserData
{
    using System;
    using Forcas.Domain.UserData;
    using NUnit.Framework;

    [TestFixture]
    public class AccountNumberTests
    {
        [TestFixture]
        public class CzTests
        {
            [Test]
            public void ShouldCreateFromIban()
            {
                var number = AccountNumber.FromIban("CZ2155000000002254171001");
                Assert.AreEqual("2254171001/5500", number.LocalFormat);
            }

            [Test]
            public void ShouldThrowWithInvalidIban()
            {
                Assert.Throws<FormatException>(() => AccountNumber.FromIban("CZ9955000000002254171001"));
            }

            [Test]
            public void ShouldThrowWithInvalidLocal()
            {
                Assert.Throws<FormatException>(() => AccountNumber.FromCzLocal("AADFHOISDFH"));
            }

            [Test]
            public void ShouldThrowOnEmptyIban()
            {
                Assert.Throws<ArgumentNullException>(() => AccountNumber.FromIban(""));
            }

            [Test]
            public void ShouldThrowOnEmptyLocal()
            {
                Assert.Throws<ArgumentNullException>(() => AccountNumber.FromCzLocal(""));
            }

            [Test]
            public void ShouldCreateFromLocal()
            {
                var number = AccountNumber.FromCzLocal("2254171001/5500");
                Assert.AreEqual("CZ2155000000002254171001", number.Iban);
            }

            [Test]
            public void ShouldReformatLocal()
            {
                var number = AccountNumber.FromCzLocal("0000-2254171001/5500");
                Assert.AreEqual("2254171001/5500", number.LocalFormat);
            }


            //[Test]
            //public void ShouldThrowIfInputIsEmpty()
            //{
            //    Assert.Catch<ArgumentException>(() => AccountNumber.Parse(null));
            //}

            //[Test]
            //public void ShouldThrowIfFormatDoesNotMatch()
            //{
            //    Assert.Catch<ArgumentException>(() => AccountNumber.Parse("sdfsdfgsd456fg46sd"));
            //}

            //[Test]
            //public void ShouldParseFullNumber()
            //{
            //    var result = AccountNumber.Parse("123-567/5500");

            //    Assert.AreEqual(123, result.Prefix);
            //    Assert.AreEqual(567, result.Number);
            //    Assert.AreEqual(5500, result.BankRoutingCode);
            //}

            //[Test]
            //public void ShouldParseNumberWithoutPrefix()
            //{
            //    var result = AccountNumber.Parse("567/5500");

            //    Assert.AreEqual(567, result.Number);
            //    Assert.AreEqual(5500, result.BankRoutingCode);
            //}
        }

        [TestFixture]
        public class EqualityTests
        {
            //private AccountNumber numberA = new AccountNumber { Prefix = 123, Number = 567, BankRoutingCode = 5500};
            //private AccountNumber numberAClone = new AccountNumber { Prefix = 123, Number = 567, BankRoutingCode = 5500};
            //private AccountNumber numberB = new AccountNumber { Prefix = 123, Number = 567, BankRoutingCode = 5501};

            //[Test]
            //public void EqualsShouldReturnTrueForSameValues()
            //{
            //    Assert.IsTrue(this.numberA.Equals(this.numberAClone));
            //}

            //[Test]
            //public void EqualsShouldReturnFalseForDifferentValues()
            //{
            //    Assert.IsFalse(this.numberA.Equals(this.numberB));
            //}

            //[Test]
            //public void GetHashCodeShouldReturnSameCodesForSameValues()
            //{
            //    Assert.AreEqual(this.numberA.GetHashCode(), this.numberAClone.GetHashCode());
            //}

            //[Test]
            //public void GetHashCodeShouldReturnDifferentCodesForDifferentValues()
            //{
            //    Assert.AreNotEqual(this.numberA.GetHashCode(), this.numberB.GetHashCode());
            //}

            //[Test]
            //public void EqualOperatorShouldReturnTrueForSameValues()
            //{
            //    Assert.IsTrue(this.numberA == this.numberAClone);
            //}

            //[Test]
            //public void EqualOperatorShouldReturnFalseForDifferentValues()
            //{
            //    Assert.IsFalse(this.numberA == this.numberB);
            //}

            //[Test]
            //public void InequalOperatorShouldReturnTrueForDifferentValues()
            //{
            //    Assert.IsTrue(this.numberA != this.numberB);
            //}

            //[Test]
            //public void InequalOperatorShouldReturnFalseForSameValues()
            //{
            //    Assert.IsFalse(this.numberA != this.numberAClone);
            //}
        }

        [TestFixture]
        public class IbanCalculationTests
        {
            [Test]
            public void ValidateIbanSuccess()
            {
                const string iban = "CZ2155000000002254171001";
                Assert.IsTrue(AccountNumber.IsIbanValid(iban));
            }

            [Test]
            public void ValidateIbanInvalid()
            {
                const string iban = "CZ2155010000002254171001";
                Assert.IsFalse(AccountNumber.IsIbanValid(iban));
            }

            [Test]
            public void ValidateIbanInvalidFormat()
            {
                const string iban = "CZ21 5501 0000 0022 5417 1001";
                Assert.Throws<FormatException>(() => AccountNumber.IsIbanValid(iban));
            }

            [Test]
            public void CalculateChecksumSuccess()
            {
                var number = AccountNumber.FromCzLocal("2254171001/5500");
                Assert.AreEqual("CZ2155000000002254171001", number.Iban);
            }
        }
    }
}