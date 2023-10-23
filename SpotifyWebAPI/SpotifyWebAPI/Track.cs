using Newtonsoft.Json;
using SpotifyWebAPI.SpotifyModel;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SpotifyWebAPI
{
    /// <summary>
    /// Spotify Track
    /// </summary>
    public class Track : BaseModel
    {
        /// <summary>
        /// The album on which the track appears. The album object includes a link in href to full information about the album. 
        /// </summary>
        public Album Album { get; set; }

        /// <summary>
        /// The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist. 
        /// </summary>
        public List<Artist> Artists { get; set; }

        /// <summary>
        ///  A list of the countries in which the track can be played, identified by their ISO 3166-1 alpha-2 code.  
        /// </summary>
        public List<string> AvailableMarkets { get; set; }
        
        /// <summary>
        /// The disc number (usually 1 unless the album consists of more than one disc).  
        /// </summary>
        public int DiscNumber { get; set; }

        /// <summary>
        /// The track length in milliseconds.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Whether or not the track has explicit lyrics (true = yes it does; false = no it does not OR unknown).  
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Known external IDs for the track.
        /// </summary>
        public ExternalId ExternalId { get; set; }

        /// <summary>
        /// Known external URLs for this track.
        /// </summary>
        public ExternalUrl ExternalUrl { get; set; }

        /// <summary>
        /// A link to the Web API endpoint providing full details of the track.
        /// </summary>
        public string HREF { get; set; }

        /// <summary>
        /// The Spotify ID for the track. 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of the track. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The popularity of the track. The value will be between 0 and 100, with 100 being the most popular. 
        /// </summary>
        public int Popularity { get; set; }

        /// <summary>
        /// A link to a 30 second preview (MP3 format) of the track.
        /// </summary>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// The number of the track. If an album has several discs, the track number is the number on the specified disc.
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        /// The object type: "track".
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Spotify URI for the track.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public Track()
        {
            Album = null;
            Artists = new List<Artist>();
            AvailableMarkets = new List<string>();
            DiscNumber = 0;
            Duration = 0;
            Explicit = false;
            ExternalId = null;
            ExternalUrl = null;
            HREF = null;
            Id = null;
            Name = null;
            Popularity = 0;
            PreviewUrl = null;
            TrackNumber = 0;
            Type = null;
            Uri = null;
        }

        /// <summary>
        /// Search for a track
        /// </summary>
        /// <param name="trackName"></param>
        /// <param name="albumName"></param>
        /// <param name="artistName"></param>
        /// <param name="year"></param>
        /// <param name="genre"></param>
        /// <param name="upc"></param>
        /// <param name="isrc"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> Search(string trackName,
            string albumName = "",
            string artistName = "",
            string year = "",
            string genre = "",
            string upc = "",
            string isrc = "",
            int limit = 50,
            int offset = 0)
        {
            string queryString = "https://api.spotify.com/v1/search?q=track:" + trackName.Replace(" ", "%20");

            if (albumName != "")
                queryString += "%20:album:" + albumName.Replace(" ", "%20");
            if (artistName != "")
                queryString += "%20:artist:" + artistName.Replace(" ", "%20");
            if (year != "")
                queryString += "%20:year:" + year.Replace(" ", "%20");
            if (genre != "")
                queryString += "%20:genre:" + genre.Replace(" ", "%20");
            if (upc != "")
                queryString += "%20:upc:" + upc.Replace(" ", "%20");
            if (isrc != "")
                queryString += "%20:isrc:" + isrc.Replace(" ", "%20");

            queryString += "&limit=" + limit;
            queryString += "&offset=" + offset;
            queryString += "&type=track";

            string json = await HttpHelper.Get(queryString);
            var obj = JsonConvert.DeserializeObject<tracksearchresult>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.tracks.ToPOCO<Track>();
        }

        /// <summary>
        /// SearchAll
        /// </summary>
        /// <param name="trackName"></param>
        /// <param name="artistName"></param>
        /// <param name="market"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> SearchAll(string trackName, string artistName, string market)
        {
            Page<Track> tr = null;
            Page<Track> ret = new Page<Track>();

            do
            {
                string queryString;

                if (tr == null)
                {
                    queryString = "https://api.spotify.com/v1/search?q=track:" + WebUtility.UrlEncode(trackName);

                    queryString += "%20artist:" + WebUtility.UrlEncode(artistName);
                    if (market != "")
                        queryString += "&market=" + market;
                    queryString += "&limit=" + 50;
                    queryString += "&offset=" + 0;
                    queryString += "&type=track";
                }
                else
                {
                    queryString = tr.Next;
                }

                string json = await HttpHelper.Get(queryString);
                var obj = JsonConvert.DeserializeObject<tracksearchresult>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });

                tr = obj.tracks.ToPOCO<Track>();

                ret.Items.AddRange(tr.Items);
                ret.Total += tr.Total;
            }
            while (tr.Next != null);

            return ret;
        }

        /// <summary>
        /// SearchAll
        /// </summary>
        /// <param name="trackName"></param>
        /// <param name="artistNames"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> SearchAll(string trackName, List<string> artistNames)
        {
            Page<Track> ret = new Page<Track>();

            foreach (string artistName in artistNames)
            {
                Page<Track> tracks = await SearchAll(trackName, artistName, "");
                ret.Items.AddRange(tracks.Items);
                ret.Total += tracks.Total;
            }

            return ret;
        }

        /// <summary>
        /// Search for a track
        /// </summary>
        /// <param name="trackName"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> SearchAll(string trackName, AuthenticationToken token)
        {
            Page<Track> tr = null;
            Page<Track> ret = new Page<Track>();

            do
            {
                string queryString;

                if (tr == null)
                {
                    queryString = "https://api.spotify.com/v1/search?q=" + trackName.Replace(" ", "%20");

                    queryString += "&market=from_token";
                    queryString += "&limit=" + 50;
                    queryString += "&offset=" + 0;
                    queryString += "&type=track";
                }
                else
                {
                    queryString = tr.Next;
                }

                string json = await HttpHelper.Get(queryString, token);
                var obj = JsonConvert.DeserializeObject<tracksearchresult>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                });

                tr = obj.tracks.ToPOCO<Track>();

                ret.Items.AddRange(tr.Items);
                ret.Total += tr.Total;
            }
            while (tr.Next != null);

            return ret;
        }

        public static async Task<Page<Track>> SearchAll(string trackName, AuthenticationToken token, int max)
        {
            Page<Track> tr;
            Page<Track> ret = new Page<Track>();

            string queryString = "https://api.spotify.com/v1/search?q=" + WebUtility.UrlEncode(trackName);

            queryString += "&market=from_token";
            queryString += "&limit=" + max;
            queryString += "&offset=" + 0;
            queryString += "&type=track";

            string json = await HttpHelper.Get(queryString, token);
            var obj = JsonConvert.DeserializeObject<tracksearchresult>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            tr = obj.tracks.ToPOCO<Track>();

            ret.Items.AddRange(tr.Items);
            ret.Total += tr.Total;

            return ret;
        }

        /// <summary>
        /// Get a track
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        public static async Task<Track> GetTrack(string trackId)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/tracks/" + trackId);
            var obj = JsonConvert.DeserializeObject<track>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }

        /// <summary>
        /// Get several tracks
        /// </summary>
        /// <param name="trackIds"></param>
        /// <returns></returns>
        public static async Task<List<Track>> GetTracks(List<string> trackIds)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/tracks?ids=" + CreateCommaSeperatedList(trackIds));
            var obj = JsonConvert.DeserializeObject<trackarray>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            var tracks = new List<Track>();
            foreach (track item in obj.tracks)
                tracks.Add(item.ToPOCO());

            return tracks;
        }

        /// <summary>
        /// Get an album's tracks
        /// </summary>
        /// <param name="trackIds"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> GetAlbumTracks(string albumId)
        {
            return await Album.GetAlbumTracks(albumId);
        }

        /// <summary>
        /// Get an artist's top tracks
        /// </summary>
        /// <param name="trackIds"></param>
        /// <returns></returns>
        public static async Task<List<Track>> GetArtistTopTracks(string artistId, string countryCode = "US")
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/artists/" + artistId + "/top-tracks?country=" + countryCode);
            var obj = JsonConvert.DeserializeObject<trackarray>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            var tracks = new List<Track>();
            foreach (track item in obj.tracks)
                tracks.Add(item.ToPOCO());

            return tracks;
        }
    }
}
