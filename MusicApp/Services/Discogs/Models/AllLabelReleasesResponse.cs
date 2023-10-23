using System.Collections.Generic;

namespace Services.Discogs.Models
{
    internal class AllLabelReleasesResponse
    {
        public Pagination Pagination { get; set; }
        public List<Release> Releases { get; set; }
    }
}
