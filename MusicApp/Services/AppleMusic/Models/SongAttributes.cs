using System;
using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    public class SongAttributes
    {
        public List<Preview> Previews { get; set; }
        public Artwork Artwork { get; set; }
        public string ArtistName { get; set; }
        public string Url { get; set; }
        public int DiscNumber { get; set; }
        public List<string> GenreNames { get; set; }
        public int DurationInMillis { get; set; }
        public string ReleaseDate { get; set; }
        public string Name { get; set; }
        public string ISRC { get; set; }
        public bool HasLyrics { get; set; }
        public string AlbumName { get; set; }
        public SongPlayParams PlayParams { get; set; }
        public int TrackNumber { get; set; }
        public string ComposerName { get; set; }

        //public DateTime DateAdded { get; set; }
    }
}
