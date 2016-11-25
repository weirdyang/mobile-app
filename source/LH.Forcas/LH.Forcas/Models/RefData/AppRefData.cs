using System.Collections.Generic;

namespace LH.Forcas.Models.RefData
{
    public class AppRefData
    {
        public IList<Bank> Banks { get; set; }

        public IList<Currency> Currencies { get; set; }
    }
}