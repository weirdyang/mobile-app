using System.Collections.Generic;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class BudgetEntity
    {
        [PrimaryKey]
        public int BudgetId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BudgetCategoryEntity> Categories { get; set; }
    }
}
