namespace Services.AppleMusic.Models
{
    public class SongPlayParams
    {
        public string Id { get; set; }
        public string Kind { get; set; }
        public bool IsLibrary { get; set; }
        public bool Reporting { get; set; }
        public string CatalogId { get; set; }
    }
}
