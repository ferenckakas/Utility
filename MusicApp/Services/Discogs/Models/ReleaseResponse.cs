using Newtonsoft.Json;
using System.Collections.Generic;

namespace Services.Discogs.Models
{
    public class ReleaseResponse
    {
        public int Id { get; set; } // id
        public string Status { get; set; } // status
        public int Year { get; set; } // year

        [JsonProperty("resource_url")]
        public string ResourceUrl { get; set; }

        public string Uri { get; set; } // uri

        public List<Artist> Artists { get; set; } // artists

        [JsonProperty("artists_sort")]
        public string ArtistsSort { get; set; } // artists_sort

        public List<Entity> Labels { get; set; } // labels

        public List<Entity> Series { get; set; } // series

        public List<Entity> Companies { get; set; } // companies

        public List<Format> Formats { get; set; } // formats

        [JsonProperty("data_quality")]
        public string DataQuality { get; set; }

        public Community Community { get; set; } // community

        [JsonProperty("format_quantity")]
        public int FormatQuantity { get; set; }

        [JsonProperty("date_added")]
        public string DateAdded { get; set; }

        [JsonProperty("date_changed")]
        public string DateChanged { get; set; }

        [JsonProperty("num_for_sale")]
        public int NumForSale { get; set; }

        [JsonProperty("lowest_price")]
        public decimal? LowestPrice { get; set; }

        [JsonProperty("master_id")]
        public int MasterId { get; set; }

        [JsonProperty("master_url")]
        public string MasterUrl { get; set; }

        public string Title { get; set; } //title
        public string Country { get; set; } //country
        public string Released { get; set; } //released
        public string Notes { get; set; } //notes

        [JsonProperty("released_formatted")]
        public string ReleasedFormatted { get; set; } //released_formatted

        public List<Identifier> Identifiers { get; set; } // identifiers
        public List<Video> Videos { get; set; } // videos
        public List<string> Genres { get; set; } // genres
        public List<string> Styles { get; set; } // styles
        public List<Track> Tracklist { get; set; } // tracklist
        public List<Artist> ExtraArtists { get; set; } //extraartists
        public List<Image> Images { get; set; } //images
        public string thumb { get; set; } //thumb

        [JsonProperty("estimated_weight")]
        public int estimated_weight { get; set; } //estimated_weight
    }
}
