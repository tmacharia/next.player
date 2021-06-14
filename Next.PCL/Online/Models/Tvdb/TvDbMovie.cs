using Next.PCL.Entities;
using System.Collections.Generic;

namespace Next.PCL.Online.Models.Tvdb
{
    public class TvDbMovie : TvdbModel
    {
        public TvDbMovie() :base()
        {
            Studios = new List<Company>();
            Networks = new List<Company>();
            Distributors = new List<Company>();
            ProductionCompanies = new List<Company>();
        }
        public List<Company> Studios { get; set; }
        public List<Company> Networks { get; set; }
        public List<Company> Distributors { get; set; }
        public List<Company> ProductionCompanies { get; set; }
    }
}