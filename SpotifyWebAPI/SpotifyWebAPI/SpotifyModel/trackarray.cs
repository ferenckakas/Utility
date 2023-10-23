using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class trackarray
    {
        public track[] tracks { get; set; }
    }
}
