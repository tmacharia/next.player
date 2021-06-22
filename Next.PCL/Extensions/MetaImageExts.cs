using Next.PCL.Enums;
using Next.PCL.Metas;

namespace Next.PCL.Extensions
{
    public static class MetaImageExts
    {
        public static Resolution DetermineResolution(this MetaImage img)
        {
            return Resolution.WVGA;
        }
        internal static Resolution EstimateResolution(int h, int w = 0)
        {
            return Resolution.HD;
        }
    }
}