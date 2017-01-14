namespace LH.Forcas.Storage
{
    using System.Collections.Generic;

    public interface IRoamingDataRepository
    {
        IEnumerable<T> GetAll<T>();

        T GetOneById<T>(object id);
    }
}