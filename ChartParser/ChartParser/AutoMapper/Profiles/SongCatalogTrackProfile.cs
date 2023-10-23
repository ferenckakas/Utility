using AutoMapper;
using Helper;
using System;

namespace ChartParser.AutoMapper.Profiles
{
    class SongCatalogTrackProfile : Profile
    {
        public SongCatalogTrackProfile()
        {
            CreateMap<Services.AppleMusic.Models.Song, CatalogTrack>()
                .ForMember(dest => dest.CatalogId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ArtworkUrl, opt => opt.MapFrom(src => src.Attributes.Artwork.Url))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Attributes.Name))
                .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Attributes.ArtistName))
                .ForMember(dest => dest.Album, opt => opt.MapFrom(src => src.Attributes.AlbumName))
                .ForMember(dest => dest.TotalTime, opt => opt.MapFrom(src => Math.Round((double)src.Attributes.DurationInMillis/1000)))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Attributes.ReleaseDate))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => string.Join(" ", src.Attributes.GenreNames)));
        }
    }
}
