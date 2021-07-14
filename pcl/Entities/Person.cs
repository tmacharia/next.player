namespace Next.PCL.Entities
{
    public class Person : RootPersonEntity
    {
        public Person() :base()
        { }
        public Person(Person person) :base(person)
        { }
        public virtual string Role { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1} as {2}", Id, Name, Role);
        }
    }
}