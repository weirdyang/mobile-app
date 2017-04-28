using LH.Forcas.Banking.Exceptions;
using LH.Forcas.Banking.Providers.Cz.Fio;

namespace LH.Forcas.Tests.Integration.Banks.Cz.Fio
{
    using System;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class FioOutputParserTests
    {
        protected Stream SampleJsonStream;
        protected Stream SampleFailingJsonStream;
        protected FioOutputParser Parser;

        [SetUp]
        public void Setup()
        {
            var jsonPath = Extensions.GetContentFilePath(@"Integration\Banks\Cz\Fio\TransactionParsingSample.json");
            var failingJsonPath =
                Extensions.GetContentFilePath(@"Integration\Banks\Cz\Fio\TransactionParsingSample-Fail.json");

            this.SampleJsonStream = File.OpenRead(jsonPath);
            this.SampleFailingJsonStream = File.OpenRead(failingJsonPath);

            this.Parser = new FioOutputParser();
        }

        [TearDown]
        public void TearDown()
        {
            this.SampleJsonStream.Dispose();
        }

        [TestFixture]
        public class SampleParsingTests : FioOutputParserTests
        {
            [Test]
            public void ShouldParseAccountInfo()
            {
                var result = this.Parser.Parse(this.SampleJsonStream);

                Assert.IsNotNull(result.Info);
                Assert.AreEqual("2400222222", result.Info.AccountId);
                Assert.AreEqual("2010", result.Info.BankId);
                Assert.AreEqual("CZK", result.Info.Currency);
                Assert.AreEqual("CZ7920100000002400222222", result.Info.Iban);
                Assert.AreEqual("FIOBCZPPXXX", result.Info.Bic);
                Assert.AreEqual(195m, result.Info.OpeningBalance);
                Assert.AreEqual(195.01m, result.Info.ClosingBalance);
                Assert.AreEqual(new DateTime(2012, 6, 26), result.Info.DateStart);
                Assert.AreEqual(new DateTime(2012, 6, 30), result.Info.DateEnd);
                Assert.AreEqual(2012, result.Info.YearList);
                Assert.AreEqual(1, result.Info.IdList);
                Assert.AreEqual(1148734530, result.Info.IdFrom);
                Assert.AreEqual(1149190193, result.Info.IdTo);
                Assert.AreEqual(1149190192, result.Info.IdLastDownload);
            }

            [Test]
            public void ShouldParseTransactions()
            {
                var result = this.Parser.Parse(this.SampleJsonStream);

                Assert.IsNotNull(result.TransactionList);
                Assert.IsNotNull(result.TransactionList.Transactions);
                Assert.AreEqual(3, result.TransactionList.Transactions.Length);

                var testedTransaction =
                    result.TransactionList.Transactions.Single(x => x.BankTransactionId.Value == 1148734530);

                Assert.AreEqual("Příjem převodem uvnitř banky", testedTransaction.TransactionType.Value);
                Assert.AreEqual("Some dummy info", testedTransaction.AdditionalInfo.Value);
                Assert.AreEqual(1.00m, testedTransaction.Amount.Value);
                Assert.AreEqual("Some description", testedTransaction.TransactionDescription.Value);

                Assert.AreEqual(0558, testedTransaction.ConstantSymbol.Value);
                Assert.AreEqual(123456, testedTransaction.VariableSymbol.Value);
                Assert.AreEqual(789456, testedTransaction.SpecificSymbol.Value);

                Assert.AreEqual("2900233333", testedTransaction.CounterPartyAccountNumber.Value);
                Assert.AreEqual("UNCRITMMXXX", testedTransaction.CounterPartyBankBIC.Value);
                Assert.AreEqual("2010", testedTransaction.CounterPartyBankRoutingCode.Value);
                Assert.AreEqual("Fio banka, a.s.", testedTransaction.CounterPartyBankName.Value);
                Assert.AreEqual("Pavel, Novák", testedTransaction.CounterPartyName.Value);

                Assert.AreEqual("CZK", testedTransaction.CurrencyCode.Value);
                Assert.AreEqual(new DateTime(2012, 6, 26), testedTransaction.Date.Value);
                Assert.AreEqual("Zpráva", testedTransaction.Note.Value);
                Assert.AreEqual("Comment", testedTransaction.UserComment.Value);
                Assert.AreEqual(2105685816, testedTransaction.UserActionId.Value);
                Assert.AreEqual("Pepa Novák", testedTransaction.TransactionOwner.Value);
            }

            [Test]
            public void ShouldThrowIfParsingFails()
            {
                try
                {
                    this.Parser.Parse(this.SampleFailingJsonStream);
                    Assert.Fail();
                }
                catch (BankPayloadFormatException)
                {
                }
            }
        }
    }
}