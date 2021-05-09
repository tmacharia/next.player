using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public interface IMetaID : IRootEntity<string>
    {
        MetaSource Source { get; set; }
    }
    public interface IMetaIDs
    {
        IList<MetaID> IDs { get; set; }
    }
}