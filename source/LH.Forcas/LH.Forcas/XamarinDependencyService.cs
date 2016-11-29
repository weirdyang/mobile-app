using LH.Forcas.Contract;
using Xamarin.Forms;

namespace LH.Forcas
{
    public class XamarinDependencyService : IDependencyService
    {
        private static readonly object DefaultServiceLock = new object();
        private static IDependencyService _defaultService;

        public static IDependencyService Default
        {
            get
            {
                if (_defaultService == null)
                {
                    lock (DefaultServiceLock)
                    {
                        if (_defaultService == null)
                        {
                            _defaultService = new XamarinDependencyService();
                        }
                    }
                }

                return _defaultService;
            }
        }

        public T Get<T>() where T : class
        {
            return DependencyService.Get<T>();
        }
    }
}
