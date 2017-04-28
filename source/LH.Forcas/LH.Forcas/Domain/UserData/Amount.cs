namespace LH.Forcas.Domain.UserData
{
    public struct Amount
    {
        public Amount(decimal value, string currencyId)
        {
            this.CurrencyId = currencyId;
            this.Value = value;
        }

        public decimal Value { get; private set; }

        public string CurrencyId { get; private set; }
    }
}