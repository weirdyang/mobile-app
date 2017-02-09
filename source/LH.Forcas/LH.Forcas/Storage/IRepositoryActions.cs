namespace LH.Forcas.Storage
{
    using System.Collections.Generic;

    public interface IRepositoryActions
    {
        IEnumerable<T> GetAll<T>();

        T GetOneById<T>(object id);

        void Delete<T>(object id);

        void Insert<T>(T item);

        void Update<T>(T item);
    }
}