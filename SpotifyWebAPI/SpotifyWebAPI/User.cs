using Newtonsoft.Json;
using SpotifyWebAPI.SpotifyModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyWebAPI
{
    public class User : BaseModel
    {
        /// <summary>
        /// The country of the user, as set in the user's account profile. An ISO 3166-1 alpha-2 country code. 
        /// This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The name displayed on the user's profile. This field is only available when the 
        /// current user has granted access to the user-read-private scope.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's email address. This field is only available when the current user has granted access to the user-read-email scope.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Known external URLs for this user.
        /// </summary>
        public ExternalUrl ExternalUrl { get; set; }

        /// <summary>
        /// A link to the Web API endpoint for this user.
        /// </summary>
        public string HREF { get; set; }

        /// <summary>
        ///  The Spotify user ID for the user.  
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The user's profile image. This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// The user's Spotify subscription level: "premium", "free", etc. This field is only available when 
        /// the current user has granted access to the user-read-private scope.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// The object type: "user"  
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Spotify URI for the user.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            Country = null;
            DisplayName = null;
            EmailAddress = null;
            ExternalUrl = null;
            HREF = null;
            Id = null;
            Images = new List<Image>();
            Product = null;
            Type = null;
            Uri = null;
        }

        /// <summary>
        /// Get public profile information about a Spotify user.
        /// </summary>
        /// <param name="userId"></param>
        public async static Task<User> GetUser(string userId)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/users/" + userId);
            var obj = JsonConvert.DeserializeObject<user>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }

        /// <summary>
        /// Get detailed profile information about the current user (including the current user’s username).
        /// </summary>
        /// <param name="userId"></param>
        public async static Task<User> GetCurrentUserProfile(AuthenticationToken token)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/me", token);
            var obj = JsonConvert.DeserializeObject<user>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }

        ///// <summary>
        ///// Get a list of the playlists owned by this Spotify user.
        ///// </summary>
        ///// <param name="token"></param>
        ///// <returns></returns>
        //public async Task<Page<Playlist>> GetPlaylists(AuthenticationToken token)
        //{
        //    return await Playlist.GetUsersPlaylists(Id, token);
        //}

        /// <summary>
        /// Get a list of the songs saved in the current Spotify user’s “Your Music” library.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<Page<Track>> GetUsersSavedTracks(AuthenticationToken token, int limit = 20, int offset = 0)
        {
            string json = await HttpHelper.Get("https://api.spotify.com/v1/me/tracks?limit=" + limit + "&offset=" + offset, token);
            var obj = JsonConvert.DeserializeObject<page<savedtrack>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO<Track>();
        }

        /// <summary>
        /// Get a list of the songs saved in the current Spotify user’s “Your Music” library.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Page<Track>> GetSavedTracks(AuthenticationToken token, int limit = 20, int offset = 0)
        {
            return await GetUsersSavedTracks(token, limit, offset);
        }

        /// <summary>
        /// Save one or more tracks to the current user’s “Your Music” library.
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SaveTracks(List<string> trackIds, AuthenticationToken token)
        {
            string tracksUri = CreateCommaSeperatedList(trackIds);
            string json = await HttpHelper.Put("https://api.spotify.com/v1/me/tracks?ids=" + tracksUri, token);
        }

        /// <summary>
        /// Remove one or more tracks from the current user’s “Your Music” library.
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteTracks(List<string> trackIds, AuthenticationToken token)
        {
            string tracksUri = CreateCommaSeperatedList(trackIds);
            string json = await HttpHelper.Delete("https://api.spotify.com/v1/me/tracks?ids=" + tracksUri, token);
        }

        /// <summary>
        /// Remove one or more tracks from the current user’s “Your Music” library.
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> AreSaved(List<string> trackIds, AuthenticationToken token)
        {
            string tracksUri = CreateCommaSeperatedList(trackIds);
            string json = await HttpHelper.Get("https://api.spotify.com/v1/me/tracks/contains?ids=" + tracksUri, token);
            var obj = JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return Convert.ToBoolean(obj.ToString().Replace("{", string.Empty).Replace("[", string.Empty).Replace("}", string.Empty).Replace("]", string.Empty).Trim());
        }

        /// <summary>
        /// Check if a is already saved in the current Spotify user’s “Your Music” library.
        /// </summary>
        /// <param name="tracks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> IsSaved(string trackId, AuthenticationToken token)
        {
            var trackIds = new List<string>();
            trackIds.Add(trackId);

            return await AreSaved(trackIds, token);
        }
    }
}
