using System;
using LiteDB;

namespace LH.Forcas.Storage
{
    public interface IDbManager : IDisposable
    {
        LiteRepository LiteRepository { get; }

        void ApplyMigrations();
    }
}