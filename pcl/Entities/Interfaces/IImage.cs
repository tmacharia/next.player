using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public interface IResolution
    {
        ushort Width { get; set; }
        ushort Height { get; set; }
        Resolution Resolution { get; set; }
    }
    public interface IImage : IResolution
    {
        long Size { get; set; }
    }
    public interface IVideo : IResolution
    {
        long Size { get; set; }
    }
}