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
            CreateMap<TvSeason, TmdbSeason>(MemberList.Source);

            CreateMap<ReviewBase, TmdbReview>(MemberList.Source);

            CreateMap<SearchTv, TmdbSearch>(MemberList.None)
                .ForMember(x => x.ReleaseDate, x => x.MapFrom(f => f.FirstAirDate));
            CreateMap<SearchMovie, TmdbSearch>(MemberList.None)
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Title));

            CreateMap<TMDbLib.Objects.Movies.Cast, Entities.Cast>(MemberList.None)
                .ForMember(x => x.Id, x => x.MapFrom(f => f.Id))
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Name))
                .ForMember(x => x.Order, x => x.MapFrom(f => f.Order))
                .ForMember(x => x.Character, x => x.MapFrom(f => f.Character));
            CreateMap<TMDbLib.Objects.TvShows.Cast, Entities.Cast>(MemberList.None)
                .ForMember(x => x.Id, x => x.MapFrom(f => f.Id))
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Name))
                .ForMember(x => x.Order, x => x.MapFrom(f => f.Order))
                .ForMember(x => x.Character, x => x.MapFrom(f => f.Character));
            CreateMap<TMDbLib.Objects.General.Crew, Entities.FilmMaker>(MemberList.None)
                .ForMember(x => x.Id, x => x.MapFrom(f => f.Id))
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Name));

            CreateMap<Company, Entities.Company>(MemberList.None)
                .ForMember(x => x.Id, x => x.MapFrom(f => f.Id))
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Name))
                .ForMember(x => x.Address, x => x.MapFrom(f => f.Headquarters));
            CreateMap<Network, Entities.Company>(MemberList.None)
                .ForMember(x => x.Id, x => x.MapFrom(f => f.Id))
                .ForMember(x => x.Name, x => x.MapFrom(f => f.Name))
                .ForMember(x => x.Address, x => x.MapFrom(f => f.Headquarters));
        }
    }
}