using System.Collections.Generic;
using System.Linq;
using LH.Forcas.Domain.RefData;

namespace LH.Forcas.Sync.RefData
{
    public class RefDataUpdate
    {
        public IEnumerable<Bank> Banks { get; set; }

        public void RemoveUnchangedEntities(RefDataStatus currentStatus)
        {
            this.Banks = this.Banks.Where(x => x.LastChangedVersion > currentStatus.DataVersion);
        }
    }
}