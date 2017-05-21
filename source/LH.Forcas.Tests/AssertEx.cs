using NUnit.Framework;

namespace LH.Forcas.Tests
{
    public static class AssertEx
    {
        public static void Contains(string substr, string text)
        {
            Assert.IsTrue(text.Contains(substr), $"The text '{text}' did not contain '{substr}'.");
        }

        public static void EndsWith(string substr, string text)
        {
            Assert.IsTrue(text.EndsWith(substr), $"The text '{text}' did not end with '{substr}'.");
        }

        public static void IsOfType<T>(object obj)
        {
            Assert.AreEqual(typeof(T), obj.GetType());
        }
    }
}
