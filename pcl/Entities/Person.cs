using System;
using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class Person : RootPersonEntity
    {
        public Person() :base()
        { }
        public Person(Person person) :base(person)
        { 
            if(person != null)
            {
                Gender = person.Gender;
                Birthday = person.Birthday;
            }
        }
        public virtual string Role { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} as {2}", Id, Name, Role);
        }
    }
}