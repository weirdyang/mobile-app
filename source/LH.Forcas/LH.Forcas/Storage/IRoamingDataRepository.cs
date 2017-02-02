namespace LH.Forcas.Storage
{
    public interface IRoamingDataRepository : IRepositoryActions
    {
        IRepositoryTransaction BeginTransaction();
    }
}