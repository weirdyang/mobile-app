using System.Collections.Generic;
using SQLite;

namespace LH.Forcas.Domain.UserData
{
    public class Budget
    {
        private int year;
        private int month;
        private int budgetId;

        [PrimaryKey]
        public int BudgetId
        {
            get { return this.budgetId; }
            set
            {
                this.budgetId = value;

                if (value > 0)
                {
                    this.month = value%100;
                    this.year = (value - this.month)/100;
                }
            }
        }

        public int Month
        {
            get { return this.month; }
            set
            {
                this.month = value;
                this.SetBudgetId();
            }
        }

        public int Year
        {
            get { return this.year; }
            set
            {
                this.year = value;
                this.SetBudgetId();
            }
        }

        public List<BudgetCategory> Categories { get; set; }

        private void SetBudgetId()
        {
            if (this.year > 0 && this.month > 0)
            {
                this.budgetId = (this.year*100) + this.month;
            }
        }
    }
}