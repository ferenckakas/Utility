using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    class savedtrack
    {
        public string added_at { get; set; }
        public track track { get; set; }
    }
}
