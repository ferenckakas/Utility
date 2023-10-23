using System.Collections.Generic;

namespace Services.Discogs.Models
{
    internal class SearchResponse
    {
        public Pagination Pagination { get; set; }
        public List<Result> Results { get; set; }
    }
}
