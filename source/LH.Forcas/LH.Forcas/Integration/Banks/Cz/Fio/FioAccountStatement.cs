namespace LH.Forcas.Integration.Banks.Cz.Fio
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public class FioAccountStatementJsonWrapper
    {
        public FioAccountStatement AccountStatement { get; set; }
    }

    public class FioAccountStatement
    {
        public AccountStatementInfo Info { get; set; }

        public FioTransactionList TransactionList { get; set; }
    }

    public class AccountStatementInfo
    {
        public string AccountId { get; set; }

        public string BankId { get; set; }

        public string Currency { get; set; }

        public string Iban { get; set; }

        public string Bic { get; set; }

        public decimal OpeningBalance { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool OpeningBalanceSpecified { get; set; }

        public decimal ClosingBalance { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool ClosingBalanceSpecified { get; set; }

        public DateTime DateStart { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool DateStartSpecified { get; set; }

        public DateTime DateEnd { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool DateEndSpecified { get; set; }

        public short YearList { get; set; }

        public short IdList { get; set; }

        public int IdFrom { get; set; }

        public int IdTo { get; set; }

        public int IdLastDownload { get; set; }
    }

    public class FioTransactionList
    {
        [JsonProperty("transaction")]
        public FioTransaction[] Transactions { get; set; }
    }

    public class FioTransaction
    {
        [JsonProperty("column22")]
        public TransactionProperty<int> BankTransactionId { get; set; }

        [JsonProperty("column0")]
        public TransactionProperty<DateTime> Date { get; set; }

        [JsonProperty("column1")]
        public TransactionProperty<decimal> Amount { get; set; }

        [JsonProperty("column14")]
        public TransactionProperty<string> CurrencyCode { get; set; }

        [JsonProperty("column2")] // Can contain prefix and dash
        public TransactionProperty<string> CounterPartyAccountNumber { get; set; }

        [JsonProperty("column3")]
        public TransactionProperty<string> CounterPartyBankRoutingCode { get; set; }

        [JsonProperty("column10")]
        public TransactionProperty<string> CounterPartyName { get; set; }

        [JsonProperty("column12")]
        public TransactionProperty<string> CounterPartyBankName { get; set; }

        [JsonProperty("column4")]
        public TransactionProperty<int> ConstantSymbol { get; set; }

        [JsonProperty("column5")]
        public TransactionProperty<int> VariableSymbol { get; set; }

        [JsonProperty("column6")]
        public TransactionProperty<int> SpecificSymbol { get; set; }

        [JsonProperty("column7")]
        public TransactionProperty<string> TransactionDescription { get; set; }

        [JsonProperty("column16")]
        public TransactionProperty<string> Note { get; set; }

        [JsonProperty("column8")]
        public TransactionProperty<string> TransactionType { get; set; }

        [JsonProperty("column9")]
        public TransactionProperty<string> TransactionOwner { get; set; }

        [JsonProperty("column18")]
        public TransactionProperty<string> AdditionalInfo { get; set; }

        [JsonProperty("column25")]
        public TransactionProperty<string> UserComment { get; set; }

        [JsonProperty("column26")]
        public TransactionProperty<string> CounterPartyBankBIC { get; set; }

        [JsonProperty("column17")]
        public TransactionProperty<int> UserActionId { get; set; }
    }

    public class TransactionProperty<T>
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public short Id { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}