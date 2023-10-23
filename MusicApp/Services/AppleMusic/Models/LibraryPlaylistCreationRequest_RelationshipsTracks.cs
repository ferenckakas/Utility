using Newtonsoft.Json;
using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    internal class LibraryPlaylistCreationRequest_RelationshipsTracks
    {
        [JsonProperty("data")]
        public List<LibraryPlaylistRequestTrack> Data { get; set; }
    }
}
