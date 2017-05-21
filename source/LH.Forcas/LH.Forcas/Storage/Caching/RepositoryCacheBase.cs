using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LH.Forcas.Events;
using MvvmCross.Plugins.Messenger;

namespace LH.Forcas.Storage.Caching
{
    public abstract class RepositoryCacheBase : IDisposable
    {
        private readonly MvxSubscriptionToken trimMemorySubToken;

        protected RepositoryCacheBase(IMvxMessenger messenger)
        {
            this.trimMemorySubToken = messenger.Subscribe<TrimMemoryRequestedEvent>(this.HandleTrimMemoryRequested);
        }

        protected TVal GetThroughCache<TVal>(ref TVal cacheStore, Func<TVal> loadFunc, object lockObj)
        {
            lock (lockObj)
            {
                if (cacheStore == null)
                {
                    cacheStore = loadFunc.Invoke();
                }

                return cacheStore;
            }
        }

        protected TVal GetCollectionQueryResult<TVal, TKey>(ConcurrentDictionary<TKey, TVal> cacheStore, TKey cacheKey, Func<TVal> loadFunc, object lockObj)
        {
            lock (lockObj)
            {
                TVal result;
                if(cacheStore.TryGetValue(cacheKey, out result))
                {
                    return result;
                }

                var loadedValue = loadFunc.Invoke();
                cacheStore.AddOrUpdate(cacheKey, loadedValue, (key, val) => loadedValue);

                return loadedValue;
            }
        }

        // ReSharper disable once RedundantAssignment
        protected bool Invalidate<T>(ref T cacheStore, object lockObj) where T : class 
        {
            lock (lockObj)
            {
                if (cacheStore != null)
                {
                    cacheStore = null;
                    return true;
                }

                return false;
            }
        }

        protected bool Invalidate<TVal, TKey>(ConcurrentDictionary<TKey, TVal> dictionary, object lockObj)
        {
            lock (lockObj)
            {
                if (dictionary.Count > 0)
                {
                    dictionary.Clear();
                    return true;
                }

                return false;
            }
        }

        protected abstract IEnumerable<Func<bool>> GetTrimPriorities();

        private void HandleTrimMemoryRequested(TrimMemoryRequestedEvent evt)
        {
            var invalidateFuncs = this.GetTrimPriorities();

            foreach (var invalidateFunc in invalidateFuncs)
            {
                var result = invalidateFunc.Invoke();

                if (evt.Severity == TrimMemorySeverity.ReleaseLevel && result)
                {
                    // One level was released which is enough for ReleaseLevel
                    break;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.trimMemorySubToken.Dispose();
            }
        }
    }
}