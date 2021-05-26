using AutoMapper;
using Next.PCL.Online.Models;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.TvShows;

namespace Next.PCL.AutoMap
{
    public class TmdbMappingProfile : Profile
    {
        public TmdbMappingProfile()
        {
            CreateMap<Company, TmdbCompany>(MemberList.None);
            CreateMap<Network, TmdbCompany>(MemberList.Source);
        }
    }
}