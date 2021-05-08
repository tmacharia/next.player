using System;

namespace Next.PCL.Entities
{
    public interface IMetaImage
    {
        Uri Url { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        MetaSource Source { get; set; }
        MetaImageType Type { get; set; }
        Resolution Resolution { get; set; }
    }
}