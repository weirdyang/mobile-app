using System;
using LiteDB;

namespace LH.Forcas.Storage
{
    public interface IDbManager : IDisposable
    {
        LiteDatabase Database { get; }

        void ApplyMigrations();
    }
}