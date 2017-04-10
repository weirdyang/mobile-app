using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LiteDB;
using NUnit.Framework;

namespace LH.Forcas.Tests.Storage
{
    public class SaveLoadUtil<TDomain> where TDomain : new()
    {
        private Action<TDomain, Guid> setIdFunc;

        private readonly Func<Guid, TDomain> loadEntityFunc;
        private readonly Action<TDomain> saveEntityAction;
        private readonly IList<PropertyInfo> ignoredProperties;
        private readonly IList<PropertyConfig> testedProperties;

        public SaveLoadUtil(
            Func<Guid, TDomain> loadEntityFunc, 
            Action<TDomain> saveEntityAction)
        {
            this.loadEntityFunc = loadEntityFunc;
            this.saveEntityAction = saveEntityAction;
            this.ignoredProperties = new List<PropertyInfo>();
            this.testedProperties = new List<PropertyConfig>();
        }

        public SaveLoadUtil<TDomain> WithId(
            Action<TDomain, Guid> setValue)
        {
            this.setIdFunc = setValue;

            return this;
        }

        public SaveLoadUtil<TDomain> WithProperty<TProp>(
            Expression<Func<TDomain, TProp>> propExpression, 
            Action<TDomain, TProp> setValue,
            TProp createValue, 
            TProp updateValue)
        {
            this.testedProperties.Add(new PropertyConfig
            {
                PropertyInfo = propExpression.ExtractPropertyInfoFromLambda(),
                CreateValue = createValue,
                UpdateValue = updateValue,
            });

            return this;
        }

        public SaveLoadUtil<TDomain> IgnoreProperty<TProp>(Expression<Func<TDomain, TProp>> propExpression)
        {
            this.ignoredProperties.Add(propExpression.ExtractPropertyInfoFromLambda());

            return this;
        }

        public void Run()
        {
            this.CheckVerifyAllPropertiesAreTested();

            var id = Guid.NewGuid();

            var domain = new TDomain();

            if (this.setIdFunc != null)
            {
                Console.WriteLine("SET: ID={0}", id);
                this.setIdFunc.Invoke(domain, id);
            }

            Console.WriteLine("TEST: CREATE ENTITY");
            this.SetProperties(domain, true);
            this.saveEntityAction.Invoke(domain);

            var loadedEntity = this.loadEntityFunc.Invoke(id);
            this.VerifyProperties(loadedEntity, true);

            Console.WriteLine("TEST: UPDATE ENTITY");
            this.SetProperties(domain, false);
            this.saveEntityAction.Invoke(domain);

            loadedEntity = this.loadEntityFunc.Invoke(id);
            this.VerifyProperties(loadedEntity, false);
        }

        private void SetProperties(TDomain domain, bool useCreateValues)
        {
            foreach (var propertyDefinition in this.testedProperties)
            {
                var value = useCreateValues ? propertyDefinition.CreateValue : propertyDefinition.UpdateValue;
                propertyDefinition.PropertyInfo.SetValue(domain, value);

                Console.WriteLine("SET: {0}={1}", propertyDefinition.PropertyInfo.Name, value);
            }
        }

        private void VerifyProperties(TDomain domain, bool useCreateValues)
        {
            foreach (var property in this.testedProperties)
            {
                var expectedValue = useCreateValues 
                    ? property.CreateValue 
                    : property.UpdateValue;

                var loadedValue = property.PropertyInfo.GetValue(domain);

                Console.WriteLine("COMPARE: {0} ({1} == {2})", property.PropertyInfo.Name, expectedValue, loadedValue);

                Assert.IsNotNull(loadedValue);
                Assert.AreEqual(expectedValue, loadedValue);
            }
        }

        private void CheckVerifyAllPropertiesAreTested()
        {
            var missingProperties = typeof(TDomain).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<BsonIgnoreAttribute>() == null)
                .Except(this.ignoredProperties)
                .Except(this.testedProperties.Select(x => x.PropertyInfo))
                .ToArray();

            if (missingProperties.Any())
            {
                var names = string.Join(", ", missingProperties.Select(x => x.Name));
                Assert.Fail("Properties {0} are not tested.", names);
            }
        }

        private class PropertyConfig
        {
            public PropertyInfo PropertyInfo { get; set; }

            public object CreateValue { get; set; }

            public object UpdateValue { get; set; }
        }
    }
}