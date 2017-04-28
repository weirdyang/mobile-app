using LH.Forcas.iOS.Storage;
using Xamarin.Forms;

[assembly:Dependency(typeof(IosPathResolver))]

namespace LH.Forcas.iOS.Storage
{
    using System;
    using System.IO;
    using LH.Forcas.Storage;

    public class IosPathResolver : IPathResolver
    {
        public void Initialize()
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            this.DbFilePath = Path.Combine(libFolder, "Forcas.ldb");
        }

        public string DbFilePath { get; private set; }
    }
}
