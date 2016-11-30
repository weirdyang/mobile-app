using System;

namespace LH.Forcas.Domain.RefData
{
    public interface IRefDataUpdate
    {
        Type EntityType { get; }

        int Version { get; }

        object[] Data { get; }
    }
}