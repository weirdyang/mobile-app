namespace LH.Forcas.Domain.UserData
{
    // TODO: Will need separate entities for this...
    public class Transaction
    {
        public string CounterpartyName { get; set; }

        public string CounterpartyAccountNumber { get; set; }

        public string Memo { get; set; }

        public string VariableSymbol { get; set; }

        public string ConstantSymbol { get; set; }

        public string SpecificSymbol { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}