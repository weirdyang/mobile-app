namespace LH.Forcas.Domain.UserData.Authorization
{
    public class StaticTokenAuthorizationBase : BankAuthorizationBase
    {
        public string Token { get; set; }
    }
}