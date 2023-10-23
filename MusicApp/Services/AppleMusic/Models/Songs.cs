using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    internal class Songs
    {
        public string Href { get; set; }
        public string Next { get; set; }
        public List<Song> Data { get; set; }
    }
}
