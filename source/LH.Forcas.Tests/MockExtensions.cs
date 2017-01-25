namespace LH.Forcas.Tests
{
    using Moq;
    using Prism.Services;

    public static class MockExtensions
    {
        public static void SetupAlert(this Mock<IPageDialogService> dialogService, bool result)
        {
            dialogService.Setup(x => x.DisplayAlertAsync(It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>(),
                                                         It.IsAny<string>()))
                         .ReturnsAsync(result);
        }
    }
}