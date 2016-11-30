using System.Collections.Generic;

namespace LH.Forcas.Domain.UserData
{
    public class Budget
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public List<BudgetCategory> Categories { get; set; }
    }
}