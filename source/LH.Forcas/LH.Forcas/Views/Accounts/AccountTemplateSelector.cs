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

            if (item is BankProductAccount)
            {
                return this.BankProductAccountDataTemplate;
            }

            if (item is CashAccount)
            {
                return this.CashAccountDataTemplate;
            }

            return null;
        }
    }
}