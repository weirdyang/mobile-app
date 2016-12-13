using System;

namespace LH.Forcas.Domain.RefData
{
    public interface IRefDataUpdate
    {
        Type Type { get; }

        string TypeName { get; }

        int Version { get; }

        object[] Data { get; }
    }
}