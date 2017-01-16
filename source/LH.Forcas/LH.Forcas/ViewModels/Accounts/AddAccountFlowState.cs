namespace LH.Forcas.ViewModels.Accounts
{
    using System;
    using System.Threading.Tasks;
    using Domain.RefData;
    using Domain.UserData;

    public class AddAccountFlowState
    {
        public Func<Task> CancelNavigationAction { get; set; }

        public Account Account { get; set; }

        public Bank Bank { get; set; }

        public async Task NavigateToSecondStep()
        {
            if (this.Account is BankAccount)
            {
                // -> bank selection

            }
            else
            {
                // -> details page in edit mode (pass the flow as parameter, the VM will save the account)
            }
        }
    }
}