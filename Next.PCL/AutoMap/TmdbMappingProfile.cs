using AutoMapper;
using Next.PCL.Online.Models;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace Next.PCL.AutoMap
{
    public class TmdbMappingProfile : Profile
    {
        public TmdbMappingProfile()
        {
            CreateMap<Company, TmdbCompany>(MemberList.None);
            CreateMap<Network, TmdbCompany>(MemberList.Source);

            CreateMap<Movie, TmdbMovie>(MemberList.None);
            CreateMap<TvShow, TmdbShow>(MemberList.Source);

            CreateMap<ReviewBase, TmdbReview>(MemberList.Source);

            CreateMap<SearchTv, TmdbSearch>(MemberList.None)
                .ForMember(x => x.ReleaseDate, x => x.MapFrom(f => f.FirstAirDate));
            CreateMap<SearchMovie, TmdbSearch>(MemberList.None)
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Title));
        }
    }
}