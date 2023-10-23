using Newtonsoft.Json;

namespace Services.Discogs.Models
{
    internal class Pagination
    {
        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        public int Items { get; set; } // items
        public int Page { get; set; } // page
        public Urls Urls { get; set; } // urls
        public int Pages { get; set; } // pages
    }
}
