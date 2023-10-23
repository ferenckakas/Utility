using Newtonsoft.Json;
using System.Collections.Generic;

namespace Services.Discogs.Models
{
    public class Community
    {
        public int Have { get; set; } //have
        public int Want { get; set; } //want
        public Rating Rating { get; set; } //rating
        public User Submitter { get; set; } //submitter
        public List<User> Contributors { get; set; } //contributors

        [JsonProperty("data_quality")]
        public string DataQuality { get; set; }
        public string Status { get; set; } //status
    }
}
