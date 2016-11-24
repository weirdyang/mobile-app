namespace LH.Forcas.Tests
{
    using System.IO;

    public class Extensions
    {
        public static string LoadFileContents(string fileName)
        {
            var currentDir = Path.GetDirectoryName(typeof(Extensions).Assembly.Location);
            // ReSharper disable once AssignNullToNotNullAttribute
            var path = Path.Combine(currentDir, fileName);

            return File.ReadAllText(path);
        }
    }
}