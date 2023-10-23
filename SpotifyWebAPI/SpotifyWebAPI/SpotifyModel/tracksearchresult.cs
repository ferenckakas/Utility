using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class tracksearchresult
    {
        public page<track> tracks { get; set; }
    }
}
