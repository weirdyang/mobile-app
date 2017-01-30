namespace LH.Forcas.Domain.UserData.Authorization
{
    public class StaticTokenAuthorization : BankAuthorizationBase
    {
        public string Token { get; set; }
    }
}