namespace LH.Forcas.Services
{
    using Domain.UserData;
    using Storage;

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IRefDataService refDataService;
        private readonly IDeviceService deviceService;
        private readonly IRoamingDataRepository dataRepository;

        public UserSettingsService(IRoamingDataRepository dataRepository, IRefDataService refDataService, IDeviceService deviceService)
        {
            this.dataRepository = dataRepository;
            this.refDataService = refDataService;
            this.deviceService = deviceService;
        }

        public UserSettings Settings { get; private set; }

        public void Initialize()
        {
            this.Settings = this.dataRepository.GetOneById<UserSettings>(UserSettings.SingleId);

            if (this.Settings == null)
            {
                var country = this.refDataService.GetCountry(this.deviceService.CountryCode);

                this.Settings = new UserSettings();
                this.Settings.DefaultCountryId = country.CountryId;
                this.Settings.DefaultCurrencyId = country.DefaultCurrencyId;

                this.dataRepository.Insert(this.Settings);
            }
        }

        public void Save()
        {
            this.dataRepository.Update(this.Settings);
        }
    }
}