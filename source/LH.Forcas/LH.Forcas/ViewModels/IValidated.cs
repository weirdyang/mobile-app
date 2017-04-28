namespace LH.Forcas.ViewModels
{
    public interface IValidated
    {
        ValidationState Validate(string propertyName);
    }
}