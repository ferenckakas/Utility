using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class artistarray
    {
        public artist[] artists { get; set; }
    }
}
