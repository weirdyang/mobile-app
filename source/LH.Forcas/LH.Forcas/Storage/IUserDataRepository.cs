namespace LH.Forcas.Storage
{
    public interface IUserDataRepository : IRepositoryActions
    {
        IRepositoryTransaction BeginTransaction();
    }
}