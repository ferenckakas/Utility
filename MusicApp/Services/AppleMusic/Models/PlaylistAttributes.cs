using System;

namespace Services.AppleMusic.Models
{
    public class PlaylistAttributes
    {
        public bool CanEdit { get; set; }
        public DateTime DateAdded { get; set; }
        public PlaylistPlayParams PlayParams { get; set; }
        public bool HasCatalog { get; set; }
        public string Name { get; set; }
        public Description Description { get; set; }
    }
}
