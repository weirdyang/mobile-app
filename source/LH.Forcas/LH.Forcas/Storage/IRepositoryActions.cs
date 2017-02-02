namespace LH.Forcas.Storage
{
    using System.Collections.Generic;

    public interface IRepositoryActions
    {
        IEnumerable<T> GetAll<T>();

        T GetOneById<T>(object id);

        void Delete<T>(object id);

        void Save<T>(T item);
    }
}