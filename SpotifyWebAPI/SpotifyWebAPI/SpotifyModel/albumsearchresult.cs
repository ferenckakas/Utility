using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class albumsearchresult
    {
        public page<album> albums { get; set; }
    }
}
