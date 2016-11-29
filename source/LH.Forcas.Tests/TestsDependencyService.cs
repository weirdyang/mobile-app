using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LH.Forcas.Contract;

namespace LH.Forcas.Tests
{
    public class TestsDependencyService : IDependencyService
    {
        private readonly IDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();

        public void Register<T>(T instance)
        {
            this.instances[typeof(T)] = instance;
        }

        public T Get<T>() where T : class
        {
            return (T)this.instances[typeof(T)];
        }
    }
}