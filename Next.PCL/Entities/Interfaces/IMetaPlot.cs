using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public interface IMetaPlot : IBaseEntity
    {
        string Plot { get; set; }
    }
    public interface IMetaPlots
    {
        List<MetaPlot> Plots { get; set; }
    }
}