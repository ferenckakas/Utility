using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class artistsearchresult
    {
        public page<artist> artists { get; set; }
    }
}
