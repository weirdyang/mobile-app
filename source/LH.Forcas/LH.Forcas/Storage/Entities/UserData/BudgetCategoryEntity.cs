using System;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;

namespace LH.Forcas.Storage.Entities.UserData
{
    public class BudgetCategoryEntity
    {
        [ForeignKey(typeof(BudgetEntity))]
        public int BudgetId { get; set; }

        [ForeignKey(typeof(BudgetCategoryEntity))]
        public int ParentBudgetCategoryId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BudgetCategoryEntity> Children { get; set; }

        [ForeignKey(typeof(CategoryEntity))]
        public Guid CategoryId { get; set; }

        [ManyToOne]
        public CategoryEntity Category { get; set; }

        public decimal Amount { get; set; }
    }
}