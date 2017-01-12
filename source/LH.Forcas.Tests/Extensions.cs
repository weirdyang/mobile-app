namespace LH.Forcas.Tests
{
    using System;
    using System.IO;
    using Prism.Navigation;

    public static class Extensions
    {
        public static string LoadFileContents(string fileName)
        {
            var currentDir = Path.GetDirectoryName(typeof(Extensions).Assembly.Location);
            // ReSharper disable once AssignNullToNotNullAttribute
            var path = Path.Combine(currentDir, fileName);

            return File.ReadAllText(path);
        }

        public static bool HasParameter<T>(this NavigationParameters parameters, string name, T expectedValue)
            where T : IEquatable<T>
        {
            return parameters.ContainsKey(name)
                   && ((T)parameters[name]).Equals(expectedValue);
        }
    }
}