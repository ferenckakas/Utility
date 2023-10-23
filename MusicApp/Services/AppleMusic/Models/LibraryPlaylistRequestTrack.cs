using Newtonsoft.Json;

namespace Services.AppleMusic.Models
{
    internal class LibraryPlaylistRequestTrack
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
