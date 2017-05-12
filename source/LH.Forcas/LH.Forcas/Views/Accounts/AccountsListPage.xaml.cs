using Xamarin.Forms;

namespace LH.Forcas.Views.Accounts
{
    public partial class AccountsListPage
    {
        public AccountsListPage()
        {
            this.InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
        }
    }
}