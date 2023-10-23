using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class playlistsearchresult
    {
        public page<playlist> playlists { get; set; }
    }
}
