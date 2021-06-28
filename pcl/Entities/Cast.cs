namespace Next.PCL.Entities
{
    public class Cast : Person
    {
        public Cast() :base()
        { }
        public Cast(Person person, string character)
            :base(person)
        {
            Role = character;
        }

        public int Order { get; set; }
    }
}