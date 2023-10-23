using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    public class Result
    {
        public int Id { get; set; } // id
        public string Year { get; set; } // year
        public List<string> Style { get; set; } // style
        public string Thumb { get; set; } // thumb
        public string Title { get; set; } // title

        [JsonProperty("master_id")]
        public int MasterId { get; set; }

        public string Type { get; set; } // type
        public List<string> Format { get; set; } // format

        [JsonProperty("master_url")]
        public string MasterUrl { get; set; }

        public List<string> Genre { get; set; } // genre
        public string Country { get; set; } // country
        public string Uri { get; set; } // uri

        [JsonProperty("cover_image")]
        public string CoverImage { get; set; }

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }
    }
}
