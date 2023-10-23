using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpotifyWebAPI.MVCTest.Controllers
{
    public class ArtistController : Controller
    {
        /// <summary>
        /// GET: GetArtistAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetArtistAsync()
        {
            return View(await SpotifyWebAPI.Artist.GetArtist("4DWX7u8BV0vZIQSpJQQDWU"));
        }

        /// <summary>
        /// GET: GetArtistAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetArtistsAsync()
        {
            List<string> artists = new List<string>();
            artists.Add("4DWX7u8BV0vZIQSpJQQDWU"); // alter bridge

            return View(await SpotifyWebAPI.Artist.GetArtists(artists));
        }

        /// <summary>
        /// GET: GetArtistAlbumsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetArtistAlbumsAsync()
        {
            var artist = await SpotifyWebAPI.Artist.GetArtist("4DWX7u8BV0vZIQSpJQQDWU");
            var output = await artist.GetAlbums();

            return View(await output.ToList());
        }   

        /// <summary>
        /// GET: GetArtistAlbumsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTopTracksAsync()
        {
            return View(await SpotifyWebAPI.Artist.GetTopTracks("4DWX7u8BV0vZIQSpJQQDWU", "US"));
        }

        /// <summary>
        /// GET: GetRelatedArtistsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetRelatedArtistsAsync()
        {
            return View(await SpotifyWebAPI.Artist.GetRelatedArtists("4DWX7u8BV0vZIQSpJQQDWU"));
        }

        /// <summary>
        /// GET: SearchAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SearchAsync()
        {
            // get search for alter bridge 
            var output = await SpotifyWebAPI.Artist.Search("Alter Bridge");

            return View(await output.ToList());
        }
    }
}