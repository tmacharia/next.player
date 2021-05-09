using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public interface IAlias
    {
        string Value { get; set; }
    }
    public interface IAliases
    {
        IList<Alias> Aliases { get; set; }
    }
}