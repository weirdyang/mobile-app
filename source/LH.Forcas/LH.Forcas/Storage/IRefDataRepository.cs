﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LH.Forcas.Models.RefData;

namespace LH.Forcas.Storage
{
    public interface IRefDataRepository
    {
        Task<IList<T>> GetRefDataAsync<T>() where T : new();

        Task SaveRefDataUpdates(IRefDataUpdate[] updates);
    }
}