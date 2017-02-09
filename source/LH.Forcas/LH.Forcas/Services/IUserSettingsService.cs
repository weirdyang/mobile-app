namespace LH.Forcas.Services
{
    using Domain.UserData;

    public interface IUserSettingsService
    {
        UserSettings Settings { get; }

        void Initialize();

        void Save();
    }
}
