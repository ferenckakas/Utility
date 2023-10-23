using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SpotifyWebAPI;

namespace SpotifyWebAPI.Test.Authentication
{
    public partial class Callback : System.Web.UI.Page
    {
        public AuthenticationToken AuthenticationToken
        {
            get
            {
                return new AuthenticationToken()
                    {
                        AccessToken = (string)ViewState["AccessToken"],
                        ExpiresOn = (DateTime)ViewState["ExpiresOn"],
                        RefreshToken = (string)ViewState["RefreshToken"],
                        TokenType = (string)ViewState["TokenType"], 
                    };
            }
            set
            {
                ViewState["AccessToken"] = value.AccessToken;
                ViewState["ExpiresOn"] = value.ExpiresOn;
                ViewState["RefreshToken"] = value.RefreshToken;
                ViewState["TokenType"] = value.TokenType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void Page_Load(object sender, EventArgs e)
        {
            ValidateSession.Validate(this.Context, this.Response);

            if (!IsPostBack)
            {
                if (Request["code"] == null)
                    Response.Redirect("~/Authentication/Default.aspx");

                if (Request["code"] == string.Empty)
                    Response.Redirect("~/Authentication/Default.aspx");

                // given the volatile nature of asp.net static best practice might to be set these values each time in a web application
                // if you were using any other platform setting them once will work
                SpotifyWebAPI.Authentication.ClientId = Session["clientid"].ToString();
                SpotifyWebAPI.Authentication.ClientSecret = Session["clientsecret"].ToString();
                SpotifyWebAPI.Authentication.RedirectUri = Session["redirecturi"].ToString();

                // before you can access an authenticated method, you'll need to retrieve an access token
                // this token must be passed to each method that requires authentication
                // also, im storing this in the view state so i can retrieve it again later
                AuthenticationToken = await SpotifyWebAPI.Authentication.GetAccessToken(Request["code"]);
            }

            // reset the display
            output.Text = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userGetUser_Click(object sender, EventArgs e)
        {
            // get my profile, returns a basic profile
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetUser("tinyioda");
            output.Text = VarDump.Dump(user, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userGetCurrentUserProfile_Click(object sender, EventArgs e)
        {
            // get the user you just logged in with
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);
            output.Text = VarDump.Dump(user, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userGetPlaylist_Click(object sender, EventArgs e)
        {
            // get the user you just logged in with
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // get all the playlists
            SpotifyWebAPI.Page<Playlist> playlists = await user.GetPlaylists(this.AuthenticationToken);

            // get the first playlist in the list
            // yes i know this is redundant but i am testing the method GetPlaylist()
            SpotifyWebAPI.Playlist playlist = await SpotifyWebAPI.Playlist.GetPlaylist(user, playlists.Items.FirstOrDefault().Id, this.AuthenticationToken);
            output.Text += VarDump.Dump(playlist, 0);

            // get the tracks
            // not by using GetPlaylist() in the previous command you already get the tracks, i'm just doing more testing here.
            SpotifyWebAPI.Page<PlaylistTrack> tracks = await playlist.GetPlaylistTracks(this.AuthenticationToken);
            output.Text += VarDump.Dump(tracks, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userGetPlaylists_Click(object sender, EventArgs e)
        {
            // get the user you just logged in with
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // get this persons playlists
            SpotifyWebAPI.Page<Playlist> playlists = await user.GetPlaylists(this.AuthenticationToken);

            output.Text = VarDump.Dump(playlists, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userGetSavedTracks_Click(object sender, EventArgs e)
        {
            // get the user you just logged in with
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);
            SpotifyWebAPI.Page<SpotifyWebAPI.Track> tracks = await user.GetSavedTracks(this.AuthenticationToken);

            output.Text = VarDump.Dump(tracks, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userSaveTracks_Click(object sender, EventArgs e)
        {
            // get alter bridge's blackbird album tracks
            var output = await SpotifyWebAPI.Album.GetAlbumTracks("0Fk4lWAADmFMmuW6jp6xyE");

            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // send the tracks to spotify
            await user.SaveTracks(output.Items.Select(i => i.Id).ToList(), this.AuthenticationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userDeleteTracks_Click(object sender, EventArgs e)
        {
            // get alter bridge's blackbird album tracks
            var output = await SpotifyWebAPI.Album.GetAlbumTracks("0Fk4lWAADmFMmuW6jp6xyE");

            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // send the tracks to spotify
            await user.DeleteTracks(output.Items.Select(i => i.Id).ToList(), this.AuthenticationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userIsTrackSaved_Click(object sender, EventArgs e)
        {
            // get alter bridge's blackbird album tracks
            var tracks = await SpotifyWebAPI.Album.GetAlbumTracks("0Fk4lWAADmFMmuW6jp6xyE");

            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // check to see if the track is saved (ties that bind, alter bridge)
            var isSaved = await user.IsSaved(tracks.Items.FirstOrDefault().Id, this.AuthenticationToken);
            output.Text = isSaved.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userCreatePlaylist_Click(object sender, EventArgs e)
        {
            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // create the playlist, this method also returns the playlist
            var newPlaylist = await SpotifyWebAPI.Playlist.CreatePlaylist(user.Id, "test", true, this.AuthenticationToken);
            output.Text = VarDump.Dump(newPlaylist, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userAddToPlaylist_Click(object sender, EventArgs e)
        {
            // get the user you just logged in with
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // get all the playlists
            SpotifyWebAPI.Page<Playlist> playlists = await user.GetPlaylists(this.AuthenticationToken);

            // get the first playlist in the list
            // yes i know this is redundant but i am testing the method GetPlaylist()
            SpotifyWebAPI.Playlist playlist = await SpotifyWebAPI.Playlist.GetPlaylist(user, playlists.Items.FirstOrDefault().Id, this.AuthenticationToken);

            // get alter bridge's blackbird album tracks
            var tracks = await SpotifyWebAPI.Album.GetAlbumTracks("0Fk4lWAADmFMmuW6jp6xyE");

            // add the tracks to the playlist
            await playlist.AddTracks(tracks.Items, this.AuthenticationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userUpdatePlaylist_Click(object sender, EventArgs e)
        {
            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // get all the playlist
            var playlists = await user.GetPlaylists(this.AuthenticationToken);

            // try and find the one we just created
            var playlist = playlists.Items.Where(i => i.Name == "test").FirstOrDefault();
            await playlist.UpdatePlaylist("new name", false, this.AuthenticationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userBrowseNewAlbums_Click(object sender, EventArgs e)
        {
            // get all new releases
            var newReleases = await SpotifyWebAPI.Browse.GetNewReleases(this.AuthenticationToken);

            // display
            output.Text = VarDump.Dump(newReleases, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void userBrowserFeaturedPlaylists_Click(object sender, EventArgs e)
        {
            // get all new releases
            var featuredPlaylists = await SpotifyWebAPI.Browse.GetFeaturedPlaylists(this.AuthenticationToken);

            // display
            output.Text = VarDump.Dump(featuredPlaylists, 0);
        }
    }
}