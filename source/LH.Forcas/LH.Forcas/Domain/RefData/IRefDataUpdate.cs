using System;

namespace LH.Forcas.Domain.RefData
{
    public interface IRefDataUpdate
    {
        Type DomainType { get; }

        int Version { get; }

        object[] Data { get; }
    }
}