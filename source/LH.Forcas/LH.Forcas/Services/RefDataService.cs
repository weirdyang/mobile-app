using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LH.Forcas.Contract;
using LH.Forcas.Models.RefData;

namespace LH.Forcas.Services
{
    public class RefDataService : IRefDataService
    {
        private readonly ICrashReporter crashReporter;
        private readonly IRefDataDownloader downloader;
        private readonly IRefDataRepository repository;

        private readonly SemaphoreSlim cacheSemaphore = new SemaphoreSlim(1, 1);
        private readonly IDictionary<Type, object> cache = new Dictionary<Type, object>();

        public RefDataService() : this(XamarinDependencyService.Default)
        {
        }

        public RefDataService(IDependencyService dependencyService)
        {
            this.crashReporter = dependencyService.Get<ICrashReporter>();
            this.downloader = dependencyService.Get<IRefDataDownloader>();
            this.repository = dependencyService.Get<IRefDataRepository>();
        }

        public async Task<IEnumerable<T>> GetRefDataAsync<T>() where T : new()
        {
            await this.cacheSemaphore.WaitAsync();

            object result;
            if (this.cache.TryGetValue(typeof(T), out result))
            {
                return (IEnumerable<T>)result;
            }

            IEnumerable<T> typedResult;

            try
            {
                typedResult = await this.repository.GetRefDataAsync<T>();
            }
            catch (Exception ex)
            {
                this.cacheSemaphore.Release();
                this.crashReporter.ReportFatal(ex);
                throw;
            }

            this.cache.Add(typeof(T), typedResult);
            this.cacheSemaphore.Release();

            return typedResult;
        }

        public async Task UpdateRefDataAsync()
        {
            try
            {
                var lastSyncTime = DateTime.MaxValue; // TODO: !!!

                IRefDataUpdate[] updates;
                try
                {
                     updates = await this.downloader.GetRefDataUpdates(lastSyncTime);
                }
                catch (Exception ex)
                {
                    this.crashReporter.ReportException(ex);
                    return;
                }

                if (updates == null || !updates.Any())
                {
                    return; // No updates available
                }

                await this.repository.SaveRefDataUpdates(updates);

                this.cache.Clear(); // Invalidate cache
            }
            catch (Exception ex)
            {
                this.crashReporter.ReportException(ex);
                throw;
            }
        }
    }
}