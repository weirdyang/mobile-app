using System;
using System.Threading.Tasks;
using LH.Forcas.ViewModels.Dashboard;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels
{
    public class RootViewModel : MvxViewModel<RootViewModelParams>
    {
        public const string DashboardTabName = "DashboardTab";
        public const string BudgetTabName = "BudgetTab";
        public const string AddCashTabName = "AddCashTab";
        public const string AccountsTabName = "AccountsTab";
        public const string MoreTabName = "MoreTab";

        public RootViewModel()
        {
            this.DashboardViewModel = new DashboardViewModel();

            this.SwitchToDashboardCommand = new MvxCommand(() => this.OnRequestSwitch(DashboardTabName));
            this.SwitchToBudgetCommand = new MvxCommand(() => this.OnRequestSwitch(BudgetTabName));
            this.SwitchToAddCashCommand = new MvxCommand(() => this.OnRequestSwitch(AddCashTabName));
            this.SwitchToAccountsCommand = new MvxCommand(() => this.OnRequestSwitch(AccountsTabName));
            this.SwitchToMoreCommand = new MvxCommand(() => this.OnRequestSwitch(MoreTabName));
        }

        public event EventHandler<string> RequestSwitch;

        public IMvxViewModel SelectedViewModel { get; set; }

        public IMvxCommand SwitchToDashboardCommand { get; }

        public IMvxCommand SwitchToBudgetCommand { get; }

        public IMvxCommand SwitchToAddCashCommand { get; }

        public IMvxCommand SwitchToAccountsCommand { get; }

        public IMvxCommand SwitchToMoreCommand { get; }

        public DashboardViewModel DashboardViewModel { get; }

        public override Task Initialize(RootViewModelParams parameters)
        {
            if (parameters != null && !string.IsNullOrEmpty(parameters.RequestedTabName))
            {
                this.OnRequestSwitch(parameters.RequestedTabName);
            }

            return Task.FromResult(0);
        }

        protected virtual void OnRequestSwitch(string tabName)
        {
            this.RequestSwitch?.Invoke(this, tabName);
        }
    }

    public class RootViewModelParams
    {
        public string RequestedTabName { get; set; }
    }
}