using AutoMapper;
using Next.PCL.Metas;
using Next.PCL.Online.Models.Tvdb;

namespace Next.PCL.AutoMap
{
    public class TvdbMappingProfile : Profile
    {
        public TvdbMappingProfile()
        {
            CreateMap<TvdbModel, TvDbShow>(MemberList.Source);
            CreateMap<TvDbShow, TvdbModel>(MemberList.Destination);

            CreateMap<TvdbModel, TvDbMovie>(MemberList.Source);
            CreateMap<TvDbMovie, TvdbModel>(MemberList.Destination);
        }
    }
    public class TvdbImageResolver : IValueResolver<TvdbModel, TvDbShow, MetaImageNx>
    {
        public MetaImageNx Resolve(TvdbModel source, TvDbShow destination, MetaImageNx destMember, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}