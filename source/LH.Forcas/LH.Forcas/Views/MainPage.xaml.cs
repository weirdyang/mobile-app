namespace LH.Forcas.Views
{
    public partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.BindingContext = new DummyCtx();
        }

        private class DummyCtx
        {
            
        }
    }
}
