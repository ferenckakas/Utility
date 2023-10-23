using System.Collections.Generic;

namespace Services.Discogs.Models
{
    public class Format
    {
        public string Name { get; set; } //name
        public string Qty { get; set; } //qty
        public List<string> Descriptions { get; set; } //descriptions
    }
}
