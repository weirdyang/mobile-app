using System.Collections.Generic;
using LiteDB;

namespace LH.Forcas.Domain.UserData
{
    public class Budget : UserEntityBase<int>
    {
        public override int Id
        {
            get { return this.Year*100 + this.Month; }
            set
            {
                this.Year = value/100;
                this.Month = value%100;
            }
        }

        public int Month { get; set; }

        public int Year { get; set; }

        public List<BudgetCategory> Categories { get; set; }

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }
}