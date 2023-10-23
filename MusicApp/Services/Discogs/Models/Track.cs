using Newtonsoft.Json;
using System.Collections.Generic;

namespace Services.Discogs.Models
{
    public class Track
    {
        public string Position { get; set; } //position

        [JsonProperty("type_")]
        public string Type { get; set; } //type_
        public List<Artist> Artists { get; set; } //artists
        public string Title { get; set; } //title
        public List<Artist> ExtraArtists { get; set; } //extraartists
        public string Duration { get; set; } //duration
    }
}
