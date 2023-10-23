using AutoMapper;
using Newtonsoft.Json;

namespace ChartParser.AutoMapper.Profiles
{
    class ResultDiscogsSearchResultProfile : Profile
    {
        public ResultDiscogsSearchResultProfile()
        {
            CreateMap<Services.Discogs.Models.Result, Entities.DiscogsSearchResult>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Genre)))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Style)))
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Format)));
        }
    }
}
