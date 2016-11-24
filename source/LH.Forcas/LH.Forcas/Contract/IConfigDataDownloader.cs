using System.Collections.Generic;
using System.Threading.Tasks;

namespace LH.Forcas.Contract
{
    public interface IConfigDataDownloader
    {
        Task<Dictionary<string, byte[]>> GetUpdatedFiles();
    }
}