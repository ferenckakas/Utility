using AutoMapper;
using ChartParser.AutoMapper.Profiles;
using Common.Models;
using Helper;

namespace ChartParser
{
    public static class Global
    {
        public static IMapper Mapper { get; set; }

        public static void Run()
        {
            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Track, SongTitle>();
                    cfg.AddProfile<SongCatalogTrackProfile>();
                    cfg.AddProfile<DiscogsSearchResultResultProfile>();
                    cfg.AddProfile<ResultDiscogsSearchResultProfile>();
                });

            Mapper = config.CreateMapper();
        }
    }
}
