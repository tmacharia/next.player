using static System.Environment;

namespace Next.PCL
{
    public class FileSys
    {
        internal static string AppFolder => GetFolderPath(SpecialFolder.LocalApplicationData);
    }
}