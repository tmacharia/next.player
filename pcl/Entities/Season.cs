using System;
using System.Collections.Generic;

namespace Next.PCL.Entities
{
    public class Season : MetaCommonA
    {
        public Season()
        {
            Episodes = new List<Episode>();
        }
        public int Number { get; set; }
        public DateTime? EndDate { get; set; }

        public List<Episode> Episodes { get; set; }
    }
}