using System;
using System.IO;
using System.Reflection;

namespace LH.Forcas.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetSiblingResourceName(this Type type, string fileName)
        {
            return $"{type.Namespace}.{fileName}";
        }

        public static string GetManifestResourceContentAsText(this Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}