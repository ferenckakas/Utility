using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    public class Tracks
    {
        public List<Song> Data { get; set; }
        public string Href { get; set; }
        public string Next { get; set; }
        public Meta Meta { get; set; }
    }
}
