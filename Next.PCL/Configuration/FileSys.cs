using System.IO;
using static System.Environment;

namespace Next.PCL
{
    public class FileSys
    {
        internal static string AppFolder => GetFolderPath(SpecialFolder.LocalApplicationData);
        internal static string SettingsFolder => Path.Combine(AppFolder, "Settings");
    }
}