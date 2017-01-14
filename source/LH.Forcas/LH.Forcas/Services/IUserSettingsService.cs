namespace LH.Forcas.Services
{
    public interface IUserSettingsService
    {
        string SyncProviderName { get; set; }

        string CountryCode { get; set; }
    }
}
