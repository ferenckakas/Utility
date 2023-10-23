namespace Services.AppleMusic.Models
{
    public class Song
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }
        public SongAttributes Attributes { get; set; }
    }
}
