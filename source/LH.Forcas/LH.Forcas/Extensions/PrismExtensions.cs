namespace LH.Forcas.Extensions
{
    using System.Threading.Tasks;
    using Localization;
    using Prism.Services;

    public static class PrismExtensions
    {
        public static async Task DisplayErrorAlert(this IPageDialogService pageDialogService, string description)
        {
            // This could be changed into a confirm box that would report the error to our storage
            await pageDialogService.DisplayAlertAsync(
                  AppResources.AlertDialog_ErrorTitle,
                  description,
                  AppResources.AlertDialog_OK);
        }
    }
}