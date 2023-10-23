using Newtonsoft.Json;

namespace Services.AppleMusic.Models
{
    internal class LibraryPlaylistCreationRequest_Attributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
