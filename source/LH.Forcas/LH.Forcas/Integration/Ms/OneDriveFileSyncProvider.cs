using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;

namespace LH.Forcas.Integration.Ms
{
    public class OneDriveFileSyncProvider
    {
        public void Dummy()
        {
            
        }

        private class AuthProvider : IAuthenticationProvider
        {
            public Task AuthenticateRequestAsync(HttpRequestMessage request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}