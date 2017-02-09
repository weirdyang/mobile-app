namespace LH.Forcas.Domain.RefData
{
    public class Currency : IIsActive
    { 
        public string CurrencyId { get; set; }

        public string DisplayFormat { get; set; }

        public bool IsActive { get; set; }
    }
}