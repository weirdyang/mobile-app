namespace LH.Forcas.Domain.RefData
{
    public class Currency : IIsActive
    { 
        public string CurrencyId { get; set; }

        public string Symbol { get; set; }

        public string DisplayName { get; set; }

        public PrefferedCcySymbolLocation PreferedSymbolPosition { get; set; }

        public bool IsActive { get; set; }
    }

    public enum PrefferedCcySymbolLocation
    {
        Before,
        After
    }
}