using System;

namespace LH.Forcas.Views.Accounts
{
    using Domain.UserData;
    using Xamarin.Forms;

    public class AccountImageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BankProductAccountDataTemplate { get; set; }

        public DataTemplate CashAccountDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is Account))
            {
                return null;
            }

            if (item is BankAccount)
            {
                return this.BankProductAccountDataTemplate;
            }

            if (item is CashAccount)
            {
                return this.CashAccountDataTemplate;
            }

            throw new NotSupportedException($"The account type {item.GetType()} is not supported.");
        }
    }
}