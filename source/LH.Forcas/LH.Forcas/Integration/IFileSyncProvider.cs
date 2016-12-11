namespace LH.Forcas.Integration
{
    public interface IFileSyncProvider
    {
        string Key { get; }

        string DisplayName { get; }
    }
}