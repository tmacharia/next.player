using Next.PCL.Enums;

namespace Next.PCL.Entities
{
    public class Company : RootPersonEntity, ICompany
    {
        public string Address { get; set; }
        public CompanyService Service { get; set; }
    }
}