using System;

namespace Next.PCL.Entities
{
    public interface IEditableEntity
    {
        DateTime? LastModified { get; set; }
    }
}