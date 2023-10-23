namespace Services.AppleMusic.Models
{
    public class Playlist
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
        public PlaylistAttributes Attributes { get; set; }
        public RelationShips RelationShips { get; set; }
    }
}
