using System;

namespace Next.PCL.Entities
{
    public abstract class EditableEntity : BaseEntity, IEditableEntity
    {
        public DateTime? LastModified { get; set; }
    }
    public abstract class EditableEntity<TKey> : RootEntity<TKey>, IEditableEntity
    {
        public DateTime? LastModified { get; set; }
    }
}