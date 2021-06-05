using System.IO;
using static System.Environment;

namespace Next.PCL
{
    public class FileSys
    {
        private static readonly string _appFolder = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), "next");

        internal static string AppFolder => ResolveFolder(_appFolder);

        internal static string SettingsFolder => ResolveFolder(Path.Combine(_appFolder, "Settings"));

        internal static string ResolveFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}