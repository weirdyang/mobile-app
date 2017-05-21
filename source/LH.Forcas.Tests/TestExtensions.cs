using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LH.Forcas.Extensions;
using LH.Forcas.ViewModels;

namespace LH.Forcas.Tests
{
    public static class TestExtensions
    {
        public static string GetSiblingResourceText(this object obj, string resourceName)
        {
            var fullName = obj.GetType().GetSiblingResourceName(resourceName);
            return obj.GetType().GetTypeInfo().Assembly.GetManifestResourceContentAsText(fullName);
        }

        public static Stream GetSiblingResourceStream(this object obj, string resourceName)
        {
            var fullName = obj.GetType().GetSiblingResourceName(resourceName);
            return obj.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(fullName);
        }

        public static string GetContentFilePath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, fileName);
        }

        //public static bool HasParameter<T>(this NavigationParameters parameters, string name, T expectedValue)
        //    where T : IEquatable<T>
        //{
        //    return parameters.ContainsKey(name)
        //           && ((T)parameters[name]).Equals(expectedValue);
        //}

        //public static void TestPropertyValidation<TVm, TProp>(this TVm viewModel, Expression<Func<TVm, TProp>>  propertyExpression, TProp validValue, TProp invalidValue)
        //    where TVm : DetailViewModelBase
        //{
        //    var propInfo = ExtractPropertyInfoFromLambda(propertyExpression);

        //    propInfo.SetValue(viewModel, validValue);
        //    Assert.Null(viewModel.ValidationResults[propInfo.Name]);

        //    propInfo.SetValue(viewModel, invalidValue);

        //    Assert.False(viewModel.ValidationResults.IsValid);
        //    Assert.NotNull(viewModel.ValidationResults[propInfo.Name]);

        //    propInfo.SetValue(viewModel, validValue);
        //    Assert.Null(viewModel.ValidationResults[propInfo.Name]);
        //}

        public static PropertyInfo ExtractPropertyInfoFromLambda(this LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("expression");
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
                throw new ArgumentException("expression");
            if (member.GetMethod.IsStatic)
                throw new ArgumentException("expression");

            return (PropertyInfo)body.Member;
        }

        public static string BuildErrorsString(this ValidationResults validationResults)
        {
            if (validationResults == null)
            {
                return null;
            }

            var builder = new StringBuilder();

            builder.AppendFormat("IsValid: {0}", validationResults.IsValid);
            builder.AppendLine();

            builder.AppendFormat("ErrorCount: {0}", validationResults.ErrorsCount);
            builder.AppendLine();

            foreach (var property in validationResults.Properties)
            {
                builder.AppendFormat("{0}: {1}", property, validationResults[property].ErrorMessage);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public static void WaitUntilNotBusy(this ActivityIndicatorState indicatorState, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
            {
                timeout = TimeSpan.FromSeconds(3);
            }

            var start = DateTime.Now;

            while (indicatorState.IsBusy && DateTime.Now - start <= timeout)
            {
                Task.Delay(500).Wait();
            }
        }
    }
}