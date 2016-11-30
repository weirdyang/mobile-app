namespace LH.Forcas
{
    public interface IDependencyService
    {
        T Get<T>() where T : class;
    }
}