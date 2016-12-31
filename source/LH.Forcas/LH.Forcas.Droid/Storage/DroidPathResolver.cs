using LH.Forcas.Droid.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(DroidPathResolver))]

namespace LH.Forcas.Droid.Storage
{
    using System;
    using System.IO;
    using LH.Forcas.Storage;

    public class DroidPathResolver : IPathResolver
    {
        public void Initialize()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            this.DbFilePath = Path.Combine(path, "Forcas.ldb");
        }

        public string DbFilePath { get; set; }
    }
}