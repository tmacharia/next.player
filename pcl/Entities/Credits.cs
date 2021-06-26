using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public class Credits
    {
        public Credits() : base()
        {
            Cast = new List<Cast>();
            Crew = new List<FilmMaker>();
        }
        public List<Cast> Cast { get; set; }
        public List<FilmMaker> Crew { get; set; }

        public override string ToString()
        {
            return string.Format("{0:N0} cast, {1:N0} crew", Cast.Count, Crew.Count);
        }
    }
}