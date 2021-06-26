using AutoMapper;

namespace Next.PCL.AutoMap
{
    public class AutomapperConfig
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.AddProfile<TmdbMappingProfile>();
                  cfg.AddProfile<TvdbMappingProfile>();
              });
            return config;
        }
    }
}