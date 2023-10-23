using Newtonsoft.Json;

namespace Services.AppleMusic.Models
{
    internal class LibraryPlaylistCreationRequest_Relationships
    {
        [JsonProperty("tracks")]
        public LibraryPlaylistCreationRequest_RelationshipsTracks Tracks { get; set; }
    }
}
