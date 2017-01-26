namespace LH.Forcas.Integration.Banks
{
    using System;
    using System.Reflection;
    using Domain.UserData.Authorization;

    [AttributeUsage(AttributeTargets.Class)]
    public class BankProviderInfoAttribute : Attribute
    {
        public BankProviderInfoAttribute(Type authorizationType, params string[] bankIds)
        {
            this.AuthorizationType = authorizationType;
            this.BankIds = bankIds;

            this.Validate();
        }

        public Type AuthorizationType { get; }

        public string[] BankIds { get; }

        private void Validate()
        {
            var baseTypeInfo = typeof(BankAuthorizationBase).GetTypeInfo();
            var authTypeInfo = this.AuthorizationType.GetTypeInfo();

            if (!baseTypeInfo.IsAssignableFrom(authTypeInfo) || authTypeInfo.IsAbstract)
            {
                throw new ArgumentException($"The authorizationType parameter has to be derived from {baseTypeInfo}");
            }

            if (this.BankIds == null || this.BankIds.Length == 0)
            {
                throw new ArgumentException("At least one BankId has to be provided.");
            }
        }
    }
}