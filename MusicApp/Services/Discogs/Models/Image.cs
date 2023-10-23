using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class Image
    {
        public string Type { get; set; } //type
        public string Uri { get; set; } //uri

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }

        public string Uri150 { get; set; } //uri150
        public int Width { get; set; } //width
        public int Height { get; set; } //height

    }
}
