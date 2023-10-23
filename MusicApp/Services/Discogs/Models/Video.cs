namespace Services.Discogs.Models
{
    public class Video
    {
        public string Uri { get; set; } //uri
        public string Title { get; set; } //title
        public string Description { get; set; } //description
        public int Duration { get; set; } //duration
        public bool Embed { get; set; } //embed
    }
}
