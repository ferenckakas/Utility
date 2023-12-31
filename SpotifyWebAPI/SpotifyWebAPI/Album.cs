﻿using Newtonsoft.Json;
using SpotifyWebAPI.SpotifyModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyWebAPI
{
    public class Album : BaseModel
    {
        /// <summary>
        /// The type of the album: one of "album", "single", or "compilation". 
        /// </summary>
        public AlbumType AlbumType { get; set; }

        /// <summary>
        /// The artists of the album. Each artist object includes a link in href to more detailed information about the artist.
        /// </summary>
        public List<Artist> Artists { get; set; }

        /// <summary>
        /// The markets in which the album is available: ISO 3166-1 alpha-2 country codes. Note that an album is considered available in a market when at least 1 of its tracks is available in that market.
        /// </summary>
        public List<string> AvailableMarkets { get; set; }

        /// <summary>
        /// Known external IDs for the album.
        /// </summary>
        public ExternalId ExternalId { get; set; }

        /// <summary>
        /// Known external URLs for this album.
        /// </summary>
        public ExternalUrl ExternalUrl { get; set; }

        /// <summary>
        /// A list of the genres used to classify the album. For example: "Prog Rock", "Post-Grunge". (If not yet classified, the array is empty.) 
        /// </summary>
        public List<string> Genres { get; set; }

        /// <summary>
        /// A link to the Web API endpoint providing full details of the album.
        /// </summary>
        public string HREF { get; set; }

        /// <summary>
        /// A link to the Web API endpoint providing full details of the album.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The cover art for the album in various sizes, widest first.
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// The name of the album.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The popularity of the album. The value will be between 0 and 100, with 100 being the most popular. 
        /// </summary>
        public int Popularity { get; set; }

        /// <summary>
        /// The popularity of the album. The value will be between 0 and 100, with 100 being the most popular. 
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// The precision with which release_date value is known: "year", "month", or "day".
        /// </summary>
        public string ReleaseDatePrecision { get; set; }

        /// <summary>
        /// The tracks of the album.
        /// </summary>
        public Page<Track> Tracks { get; set; }

        /// <summary>
        /// The object type: "album"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Spotify URI for the album. 
         /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Album()
        {
            AlbumType = AlbumType.Album;
            Artists = new List<Artist>();
            AvailableMarkets = new List<string>();
            ExternalId = null;
            ExternalUrl = null;
            Genres = new List<string>();
            HREF = null;
            Id = null;
            Images = new List<Image>();
            Name = null;
            Popularity = 0;
            ReleaseDate = DateTime.MinValue;
            ReleaseDatePrecision = null;
            Tracks = null;
            Type = null;
            Uri = null;
        }

        /// <summary>
        /// Search for an album
        /// </summary>
        /// <param name="albumName"></param>
        /// <param name="artistName"></param>
        /// <param name="year"></param>
        /// <param name="genre"></param>
        /// <param name="upc"></param>
        /// <param name="isrc"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static async Task<Page<Album>> Search(string albumName, 
            string artistName = "",
            string year = "",
            string genre = "",
            string upc = "",
            string isrc = "", 
            int limit = 20, 
            int offset = 0)
        {
            string queryString = "https://api.spotify.com/v1/search?q=album:" + albumName.Replace(" ", "%20");
           
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
            queryString += "&type=album";

            string json = await HttpHelper.Get(queryString);
            var obj = JsonConvert.DeserializeObject<albumsearchresult>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.albums.ToPOCO<Album>();
        }

        /// <summary>
        /// Get an album
        /// </summary>
        /// <returns></returns>
        public static async Task<Album> GetAlbum(string albumId)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/albums/" + albumId);
            var obj = JsonConvert.DeserializeObject<album>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }

        /// <summary>
        /// Get several albums
        /// </summary>
        /// <param name="albumIds"></param>
        /// <returns></returns>
        public static async Task<List<Album>> GetAlbums(List<string> albumIds)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/albums/?ids=" + CreateCommaSeperatedList(albumIds));
            var obj = JsonConvert.DeserializeObject<albumarray>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            List<Album> albums = new List<Album>();
            foreach (album item in obj.albums)
                albums.Add(item.ToPOCO());

            return albums;
        }

        /// <summary>
        /// Get an artist's albums
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public static async Task<Page<Album>> GetArtistAlbums(string artistId)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/artists/" + artistId + "/albums");
            var obj = JsonConvert.DeserializeObject<page<album>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO<Album>();
        }

        /// <summary>
        /// Get an album's tracks
        /// </summary>
        /// <param name="albumId"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> GetAlbumTracks(string albumId, int limit = 20, int offset = 0)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/albums/" + albumId + "/tracks?limit=" + limit + "&offset=" + offset);
            var obj = JsonConvert.DeserializeObject<page<track>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO<Track>();
        }

        /// <summary>
        /// Get this album's tracks
        /// </summary>
        /// <returns></returns>
        public async Task<Page<Track>> GetAlbumTracks(int limit = 20, int offset = 0)
        {
            return await GetAlbumTracks(Id, limit, offset);
        }


        /// <summary>
        /// Get a list of new releases
        /// </summary>
        /// <returns></returns>
        public async Task<Page<Album>> GetNewReleases(AuthenticationToken token, string country = "", int limit = 20, int offset = 0)
        {
            return await Browse.GetNewReleases(token, country, limit, offset);
        }
    }
}
