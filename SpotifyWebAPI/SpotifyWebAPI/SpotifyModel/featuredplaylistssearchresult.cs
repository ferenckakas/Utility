using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class featuredplaylistssearchresult
    {
        public string message { get; set; }

        public page<playlist> playlists { get; set; }
    }
}
