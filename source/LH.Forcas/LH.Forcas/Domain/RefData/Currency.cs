using LiteDB;

namespace LH.Forcas.Domain.RefData
{
    public class Currency
    { 
        public string CurrencyCode { get; set; }

        public string Symbol { get; set; }

        public string DisplayName { get; set; }

        public PrefferedCcySymbolLocation PreferedSymbolPosition { get; set; }
    }

    public enum PrefferedCcySymbolLocation
    {
        Before,
        After
    }
}