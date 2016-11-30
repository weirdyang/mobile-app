using System.Collections.Generic;

namespace LH.Forcas.Domain.UserData
{
    public class BudgetCategory
    {
        public string CategoryId { get; set; }

        public decimal Amount { get; set; }

        public List<BudgetCategory> Children { get; set; }
    }
}