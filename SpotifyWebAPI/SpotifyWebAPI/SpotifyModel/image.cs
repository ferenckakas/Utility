﻿using Newtonsoft.Json;

namespace SpotifyWebAPI.SpotifyModel
{
    [JsonObject]
    internal class image
    {
        public int ?height { get; set; }

        public string url { get; set; }

        public int ?width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Image ToPOCO()
        {
            if (!this.height.HasValue)
                return null;

            if (!this.width.HasValue)
                return null;

            return new Image() { Height = this.height.Value, Url = this.url, Width = this.width.Value };
        }
    }
}
