using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    internal class SongsResponse
    {
        public string Next { get; set; }
        public List<Song> Data { get; set; }
        public Meta Meta { get; set; }
    }
}
