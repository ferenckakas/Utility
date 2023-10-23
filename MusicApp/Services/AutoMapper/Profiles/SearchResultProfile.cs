using AutoMapper;

namespace Services.AutoMapper.Profiles
{
    class SearchResultProfile : Profile
    {
        public SearchResultProfile()
        {
            CreateMap<Google.Apis.YouTube.v3.Data.SearchResult, Common.Models.SearchResult>()
                .ForMember(dest => dest.VideoId, opt => opt.MapFrom(s => s.Id.VideoId))
                .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.Snippet.ChannelId))
                .ForMember(d => d.ChannelTitle, opt => opt.MapFrom(s => s.Snippet.ChannelTitle))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Snippet.Title))
                .ForMember(d => d.ThumbnailUrl, opt => opt.MapFrom(s => s.Snippet.Thumbnails.Default__.Url));
        }
    }
}
