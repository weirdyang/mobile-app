namespace LH.Forcas.Domain.UserData
{
    using LiteDB;

    public class UserSettings : UserEntityBase<string>
    {
        public static string SingleId = nameof(UserSettings);

        [BsonId]
        public override string Id
        {
            get { return SingleId; }
            set { }
        }

        public string DefaultCurrencyId { get; set; }

        public string DefaultCountryId { get; set; }

        // TBA: Cloud sync provider

        public override BsonValue GetIdAsBson()
        {
            return new BsonValue(this.Id);
        }
    }
}