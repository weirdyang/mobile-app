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

        public static async Task<bool> DisplayConfirmAlert(this IPageDialogService pageDialogService, string title, string descriptionFormat, params object[] args)
        {
            var description = string.Format(descriptionFormat, args);

            return await pageDialogService.DisplayAlertAsync(
                  AppResources.AlertDialog_ErrorTitle,
                  description,
                  AppResources.ConfirmDialog_Yes,
                  AppResources.ConfirmDialog_No);
        }
    }
}