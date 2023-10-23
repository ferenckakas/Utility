using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class albumarray
    {
        public album[] albums { get; set; }
    }
}
