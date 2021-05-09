namespace Next.PCL.Entities
{
    public interface IPerson : INamedEntity, IUrls, IMetaImages
    {
        
    }
    public interface IFilmMaker : IPerson
    {
        Profession Role { get; set; }
    }
    public interface ICompany : IPerson
    {
        CompanyService Service { get; set; }
    }
}