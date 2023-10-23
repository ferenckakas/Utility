using System.Collections.Generic;

namespace Services.AppleMusic.Models
{
    internal class PlaylistsResponse
    {
        public string Next { get; set; }
        public List<Playlist> Data { get; set; }
        public Meta Meta { get; set; }
    }
}
