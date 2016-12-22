namespace LH.Forcas.Storage
{
    public interface IPathResolver
    {
        void Initialize();

        string DbFilePath { get; }
    }
}