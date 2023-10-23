using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;

namespace ChartParser.AutoMapper.Profiles
{
    class DiscogsSearchResultResultProfile : Profile
    {
        public DiscogsSearchResultResultProfile()
        {
            CreateMap<Entities.DiscogsSearchResult, Services.Discogs.Models.Result>()                
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Genre)))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Style)))
                .ForMember(dest => dest.Format, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<List<string>>(src.Format)));
        }
    }
}
