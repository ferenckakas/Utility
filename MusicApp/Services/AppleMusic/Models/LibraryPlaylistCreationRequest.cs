using Newtonsoft.Json;

namespace Services.AppleMusic.Models
{
    internal class LibraryPlaylistCreationRequest
    {
        [JsonProperty("attributes")]
        public LibraryPlaylistCreationRequest_Attributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public LibraryPlaylistCreationRequest_Relationships Relationships { get; set; }
    }
}
