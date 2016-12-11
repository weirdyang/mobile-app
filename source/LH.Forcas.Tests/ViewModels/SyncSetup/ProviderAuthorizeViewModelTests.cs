using NUnit.Framework;

namespace LH.Forcas.Tests.ViewModels.SyncSetup
{
    [TestFixture]
    public class ProviderAuthorizeViewModelTests
    {
        /*
         * Back button should always work -> user changed his mind
         * When url reached -> TBA - fetch token? -> navigate to next screen
         * When internet connection is not available -> modal -> later
         * When navigation fails -> retry a couple times, retry count configured in AppConfig
         * ??? Provide refresh button for the user, disable when navigating
         */
    }
}