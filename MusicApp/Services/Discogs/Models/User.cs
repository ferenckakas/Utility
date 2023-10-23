using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class User
    {
        public string Username { get; set; } //username

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }
    }
}
