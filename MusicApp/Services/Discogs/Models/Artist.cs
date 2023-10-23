using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class Artist
    {
        public int Id { get; set; } //id
        public string Name { get; set; } //name
        public string Anv { get; set; } //anv
        public string Join { get; set; } //join
        public string Role { get; set; } //role
        public string Tracks { get; set; } //tracks

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }
    }
}
