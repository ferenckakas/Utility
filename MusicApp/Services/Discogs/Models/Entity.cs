using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class Entity
    {
        public int Id { get; set; } //id
        public string Name { get; set; } //name
        public string CatNo { get; set; } //catno

        [JsonProperty("entity_type")]
        public string EntityType { get; set; }

        [JsonProperty("entity_type_name")]
        public string EntityTypeName { get; set; }

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }
    }
}
