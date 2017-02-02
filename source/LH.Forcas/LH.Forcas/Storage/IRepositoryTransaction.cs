namespace LH.Forcas.Storage
{
    using System;
    public interface IRepositoryTransaction : IRepositoryActions, IDisposable
    {
        void Complete();
    }
}