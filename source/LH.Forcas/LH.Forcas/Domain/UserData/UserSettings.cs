namespace LH.Forcas.Domain.UserData
{
    using LiteDB;

    public class UserSettings
    {
        public static string SingleId = nameof(UserSettings);

        [BsonId]
        public string UserSettingsId => SingleId;

        public string DefaultCurrencyId { get; set; }

        public string DefaultCountryId { get; set; }

        // TBA: Cloud sync provider
    }
}