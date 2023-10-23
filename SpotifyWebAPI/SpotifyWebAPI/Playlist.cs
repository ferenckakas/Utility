using Newtonsoft.Json;
using SpotifyWebAPI.SpotifyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI
{
    public class Playlist : BaseModel
    {
        /// <summary>
        /// true if the owner allows other users to modify the playlist. Note: only non-collaborative playlists are currently returned by the Web API.
        /// </summary>
        public bool Collaborative { get; set; }

        /// <summary>
        /// The playlist description. Only returned for modified, verified playlists, otherwise null.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Known external URLs for this playlist.
        /// </summary>
        public ExternalUrl ExternalUrl { get; set; }

        /// <summary>
        /// Information about the followers of the playlist. 
        /// </summary>
        public Followers Followers { get; set; }

        /// <summary>
        /// A link to the Web API endpoint providing full details of the playlist.
        /// </summary>
        public string HREF { get; set; }

        /// <summary>
        /// The Spotify ID for the playlist. 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The playlist image. Note that this field is only returned for modified, verified playlists, otherwise the 
        /// array is empty. If returned, the source URL for the image (url) is temporary and will expire in less than a day.
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// The name of the playlist.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user who owns the playlist
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// true if the playlist is not marked as secret. 
        /// </summary>
        public bool Public { get; set; }

        /// <summary>
        /// Information about the tracks of the playlist. 
        /// </summary>
        public Page<PlaylistTrack> Tracks { get; set; }

        /// <summary>
        /// The object type: "playlist"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Spotify URI for the artist.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The Spotify URI for the artist.
        /// </summary>
        public string SnapshotId { get; set; }
        
        /// <summary>
        /// default constructor
        /// </summary>
        public Playlist()
        {
            Images = new List<Image>();
        }

        ///// <summary>
        ///// Get a list of the playlists owned by a Spotify user.
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public static async Task<Page<Playlist>> GetUsersPlaylists(User user, AuthenticationToken token)
        //{
        //    return await GetUsersPlaylists(user.Id, token);
        //}

        /// <summary>
        /// Get a list of the playlists owned by a Spotify user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Page<Playlist>> GetUsersPlaylists(string userId, AuthenticationToken token, int limit, int offset)
        {
            string querystring = "?limit=" + limit + "&offset=" + offset;

            string json = await HttpHelper.Get("https://api.spotify.com/v1/users/" + userId + "/playlists" + querystring,  token);
            var obj = JsonConvert.DeserializeObject<page<playlist>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO<Playlist>();
        }

        public static async Task<List<Playlist>> GetAllPlaylists(string userId, AuthenticationToken token)
        {
            int limit = 10;
            int offset = 0;
            int total;

            var playlists = new List<Playlist>();

            do
            {
                Page<Playlist> playlistsPage = await GetUsersPlaylists(userId, token, limit, offset);
                playlists.AddRange(playlistsPage.Items);

                total = playlistsPage.Total;
                offset += limit;
            }
            while (offset < total);

            return playlists;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Playlist> GetPlaylist(User user, string playlistId, AuthenticationToken token)
        {
            return await GetPlaylist(user.Id, playlistId, token);
        }

        /// <summary>
        /// Get a playlist owned by a Spotify user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Playlist> GetPlaylist(string userId, string playlistId, AuthenticationToken token)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/users/" + userId + "/playlists/" + playlistId, token);
            var obj = JsonConvert.DeserializeObject<playlist>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }

        /// <summary>
        /// Get full details of the tracks of a playlist owned by a Spotify user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Page<PlaylistTrack>> GetPlaylistTracks(string userId, string playlistId, AuthenticationToken token, int limit, int offset)
        {
            string querystring = "?limit=" + limit + "&offset=" + offset;

            string json = await HttpHelper.Get("https://api.spotify.com/v1/users/" + userId + "/playlists/" + playlistId + "/tracks" + querystring, token);
            var obj = JsonConvert.DeserializeObject<page<playlisttrack>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO<PlaylistTrack>();
        }

        public static async Task<List<PlaylistTrack>> GetAllPlaylistTracks(string userId, string playlistId, AuthenticationToken token)
        {
            int limit = 10;
            int offset = 0;
            int total;

            var playlistTracks = new List<PlaylistTrack>();

            do
            {
                Page<PlaylistTrack> playlistTracksPage = await GetPlaylistTracks(userId, playlistId, token, limit, offset);
                playlistTracks.AddRange(playlistTracksPage.Items);

                total = playlistTracksPage.Total;
                offset += limit;
            }
            while (offset < total);

            return playlistTracks;
        }


        /// <summary>
        /// Get full details of the tracks of this playlist
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<PlaylistTrack>> GetAllPlaylistTracks(AuthenticationToken token)
        {
            return await GetAllPlaylistTracks(Owner.Id, Id, token);
        }

        /// <summary>
        /// Add a track to a user’s playlist.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task AddTrack(string trackUri, AuthenticationToken token)
        {
            await HttpHelper.Post("https://api.spotify.com/v1/users/" + Owner.Id + "/playlists/" + Id + "/tracks?uris=" + trackUri, token);
        }

        /// <summary>
        /// Add one or more tracks to a user’s playlist.
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task AddTracks(List<Track> trackUris, AuthenticationToken token)
        {
            string tracksUri = CreateCommaSeperatedList(trackUris.Select(t => t.Uri).ToList());
            string json = await HttpHelper.Post("https://api.spotify.com/v1/users/" + Owner.Id + "/playlists/" + Id + "/tracks?uris=" + tracksUri, token);
        }

        public async Task AddTracks(AuthenticationToken token, List<string> newTrackUris)
        {
            int limit = 100;
            int offset = 0;
            int total = newTrackUris.Count;

            do
            {
                var trackUris = new List<string>();

                for (int i = offset; i < Math.Min(offset + limit, total); i++)
                {
                    trackUris.Add(newTrackUris[i]);
                }

                var urisObj = new { uris = trackUris.ToArray() };

                string jsonInput = JsonConvert.SerializeObject(urisObj);

                string json = await HttpHelper.Post($"https://api.spotify.com/v1/users/{Owner.Id}/playlists/{Id}/tracks", token.AccessToken, jsonInput);

                offset += limit;

            }
            while (offset < total);
        }

        /// <summary>
        /// Create a playlist for a Spotify user. (The playlist will be empty until you add tracks.)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="isPublic"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Playlist> CreatePlaylist(string userId, string name, bool isPublic, AuthenticationToken token)
        {
            dynamic newObject = new System.Dynamic.ExpandoObject();
            newObject.name = name;
            newObject.@public = isPublic;

            string jsonInput = JsonConvert.SerializeObject(newObject);
            string json = await HttpHelper.Post("https://api.spotify.com/v1/users/" + userId + "/playlists", token, jsonInput);
            var obj = JsonConvert.DeserializeObject<playlist>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO(); 
        }

        /// <summary>
        /// Change a playlist’s name and public/private state. (The user must, of course, own the playlist.)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="isPublic"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task UpdateUsersPlaylist(string userId, string playlistId, string name, bool isPublic, AuthenticationToken token)
        {
            dynamic newObject = new System.Dynamic.ExpandoObject();
            newObject.name = name;
            newObject.@public = isPublic;

            string jsonInput = JsonConvert.SerializeObject(newObject);
            string json = await HttpHelper.Put("https://api.spotify.com/v1/users/" + userId + "/playlists/" + playlistId, token, jsonInput);
        }        

        /// <summary>
        /// Change a playlist’s name and public/private state. (The user must, of course, own the playlist.)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="isPublic"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task UpdatePlaylist(string name, bool isPublic, AuthenticationToken token)
        {
            await UpdateUsersPlaylist(Owner.Id, Id, name, isPublic, token);
        }

        /// <summary>
        /// Remove one or more tracks from a user’s playlist.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playlistId"></param>
        /// <param name="token"></param>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public async Task RemoveTracks(AuthenticationToken token, List<Track> tracks)
        {
            int limit = 100;
            int offset = 0;
            int total = tracks.Count;

            do
            {
                var tracksList = new List<object>();

                for (int i = offset; i < Math.Min(offset+limit, total); i++)
                {
                    Track track = tracks[i];

                    var positionsList = new List<object>();
                    positionsList.Add(i-offset);

                    tracksList.Add(new { uri = track.Uri, positions = positionsList.ToArray() });
                }

                var tracksObj = new { tracks = tracksList.ToArray() };

                string jsonInput = JsonConvert.SerializeObject(tracksObj);

                string json = await HttpHelper.Delete($"https://api.spotify.com/v1/users/{Owner.Id}/playlists/{Id}/tracks", token.AccessToken, jsonInput);

                offset += limit;

            }
            while (offset < total);
        }

        /// <summary>
        /// Replace all the tracks in a playlist, overwriting its existing tracks. This powerful request can be useful for replacing tracks, re-ordering existing tracks, or clearing the playlist.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="playlistId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ReplacePlaylistTracks(string userId, string playlistId, AuthenticationToken token, List<Track> tracks)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a list of featured playlists
        /// </summary>
        /// <param name="token"></param>
        /// <param name="locale"></param>
        /// <param name="country"></param>
        /// <param name="timestamp">A timestamp in ISO 8601 format: yyyy-MM-ddTHH:mm:ss.</param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async static Task<Page<Playlist>> GetFeaturedPlaylists(AuthenticationToken token, string locale = "", string country = "", string timestamp = "", int limit = 20, int offset = 0)
        {
            return await Browse.GetFeaturedPlaylists(token, locale, country, timestamp, limit, offset);
        }
    }
}
