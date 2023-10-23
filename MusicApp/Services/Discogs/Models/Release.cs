using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class Release
    {
        public int Id { get; set; } // id
        public string Year { get; set; } // year
        public string Artist { get; set; } // artist
        public string Title { get; set; } // title
        public string Thumb { get; set; } // thumb

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }

        public string Format { get; set; } // format
        public string CatNo { get; set; } // catno
        public string Status { get; set; } // status
    }
}
