using Prism.Navigation;

namespace LH.Forcas.Tests
{
    using System.Threading.Tasks;
    using Moq;
    using Moq.Language.Flow;
    using Prism.Services;

    public static class MockExtensions
    {
        public static ISetup<INavigationService, Task> SetupNavigation(this Mock<INavigationService> mock, string pageName)
        {
            return mock.Setup(x => x.NavigateAsync(It.Is<string>(uri => uri.Contains(pageName)), null, false, true));
        }

        public static void SetupAlert(this Mock<IPageDialogService> dialogService)
        {
            dialogService.Setup(x => x.DisplayAlertAsync(It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>()))
                         .ReturnAwaitable();
        }

        public static void SetupAlert(this Mock<IPageDialogService> dialogService, bool result)
        {
            dialogService.Setup(x => x.DisplayAlertAsync(It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>()))
                         .ReturnsAsync(result);
        }

        public static ISetup<TMock, Task> ReturnAwaitable<TMock>(this ISetup<TMock, Task> setup) where TMock : class
        {
            setup.Returns(Task.FromResult(0));

            return setup;
        }
    }
}