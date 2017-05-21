using System.Threading.Tasks;
using LH.Forcas.Localization;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace LH.Forcas.Extensions
{
    public static class PrismExtensions
    {
        public static async Task DisplayErrorAlert(this IUserInteraction userInteraction, string description)
        {
            // This could be changed into a confirm box that would report the error to our storage
            await userInteraction.AlertAsync(
                  AppResources.AlertDialog_ErrorTitle,
                  description,
                  AppResources.AlertDialog_OK);
        }

        public static async Task<bool> YesNoConfirmAlert(this IUserInteraction userInteraction, string title, string descriptionFormat, params object[] args)
        {
            var description = string.Format(descriptionFormat, args);

            return await userInteraction.ConfirmAsync(
                  description,
                  AppResources.AlertDialog_ErrorTitle,
                  AppResources.ConfirmDialog_Yes,
                  AppResources.ConfirmDialog_No);
        }
    }
}