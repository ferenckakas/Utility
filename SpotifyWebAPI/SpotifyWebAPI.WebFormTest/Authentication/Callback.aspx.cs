using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SpotifyWebAPI;
using System.Globalization;

namespace SpotifyWebAPI.Test.Authentication
{
    public partial class Callback : System.Web.UI.Page
    {
        private class State
        {
            public List<string> Artists;
            public string Artist;
            public string Song;
            public string Keywords;
            public Playlist Playlist;
            public Page<SpotifyWebAPI.Track> Tracks;
        }

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
            if (Request["code"] == "Test")
                return;

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
        protected void A_Click(object sender, EventArgs e)
        {
            TextBox1.Text += ViewState["AccessToken"] + "\r\n";
            TextBox1.Text += ViewState["ExpiresOn"] + "\r\n";
            TextBox1.Text += ViewState["RefreshToken"] + "\r\n";
            TextBox1.Text += ViewState["TokenType"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Click(object sender, EventArgs e)
        {
            string[] sep = {"\r\n"};
            string[] lines = TextBox1.Text.Split(sep, StringSplitOptions.None);

            ViewState["AccessToken"] = lines[0];
            ViewState["ExpiresOn"] = DateTime.Parse(lines[1], CultureInfo.CreateSpecificCulture("en-US"));
            ViewState["RefreshToken"] = lines[2];
            ViewState["TokenType"] = lines[3];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void K_Click(object sender, EventArgs e)
        {
            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // create the playlist, this method also returns the playlist
            var playlist = await SpotifyWebAPI.Playlist.CreatePlaylist(user.Id, "test 2011", true, this.AuthenticationToken);
            //output.Text = VarDump.Dump(playlist, 0);


            string[] sep = { "\r\n" };
            string[] lines = TextBox2.Text.Split(sep, StringSplitOptions.None);

            string[] sepTab = { "\t" };
            string[] sepArtist = { " featuring ", " feat. ", " feat ", " ft. ", " ft ", " & ", ", ", ",", " , ", " ," };

            System.Web.UI.WebControls.Table table = new Table();
            table.BorderWidth = 1;


            foreach (string line in lines)
            {
                string[] parts = line.Split(sepTab, StringSplitOptions.None);
                List<string> artists = new List<string>();
                artists.Add(parts[0]);
                string[] ars = parts[0].Split(sepArtist, StringSplitOptions.None);
                artists.AddRange(ars);

                string song = parts[1];

                string keywords = "";
                //artists.ForEach(f => words += " " + f);
                keywords += artists[1];
                if (artists.Count > 2) keywords += " " + artists[2];
                keywords += " " + song;

                string keywordsEncoded = Server.UrlEncode(keywords);

                Page<SpotifyWebAPI.Track> tracks = await SpotifyWebAPI.Track.SearchAll(keywordsEncoded);

                found = false;

                var state = new State { Artists = artists, Artist = artists[0], Song = song, Keywords = keywords, Playlist = playlist, Tracks = tracks };

                tracks = FilterR(state, track => track.Artists[0].Name == artists[1] || (artists.Count > 2 && track.Artists[0].Name == artists[2]));

                state = new State { Artists = artists, Artist = artists[0], Song = song, Keywords = keywords, Playlist = playlist, Tracks = tracks };


                TableRow row = new TableRow();
                LinkButton linkButton = new LinkButton { Text = String.Format("Get {0} - {1}", artists[0], song) };
                linkButton.Click += linkButton_Click;
                TableCell buttonCell = new TableCell();
                buttonCell.Controls.Add(linkButton);
                row.Cells.Add(buttonCell);
                //row.Cells.Add(new TableCell { Text = artists[0] });
                //row.Cells.Add(new TableCell { Text = song });              
                table.Rows.Add(row);
           
            }

            TableDiv.Controls.Add(table);
            TableDiv.Controls.Add(new Label());


        }

        void linkButton_Click(object sender, EventArgs e)
        {
            string s = Request.QueryString.ToString();
            throw new NotImplementedException();
        }

        bool found = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void T_Click(object sender, EventArgs e)
        {
            // get the user
            SpotifyWebAPI.User user = await SpotifyWebAPI.User.GetCurrentUserProfile(this.AuthenticationToken);

            // create the playlist, this method also returns the playlist
            var playlist = await SpotifyWebAPI.Playlist.CreatePlaylist(user.Id, TextBoxTitle.Text, true, this.AuthenticationToken);
            //output.Text = VarDump.Dump(playlist, 0);


            string[] sep = { "\r\n" };
            string[] lines = TextBox2.Text.Split(sep, StringSplitOptions.None);

            string[] sepTab = { "\t" };
            string[] sepArtist = { " featuring ", " feat. ", " feat ", " ft. ", " ft ", " vs. ", " vs ", ", ", ",", " , ", " ," };
            string[] sepArtist2 = { " & " };
            string[] sepArtistFull = new string[sepArtist.Length + sepArtist2.Length];
            sepArtist.CopyTo(sepArtistFull, 0);
            sepArtist2.CopyTo(sepArtistFull, sepArtist.Length);

            foreach(string line in lines)
            {
                string[] parts = line.Split(sepTab, StringSplitOptions.None);
                List<string> artists = new List<string>();
                artists.Add(parts[0]);
                string[] ars = parts[0].Split(sepArtist, StringSplitOptions.None);
                foreach(string ar in ars)
                {
                    if (!artists.Contains(ar)) 
                        artists.Add(ar);
                    string[] ars2 = ar.Split(sepArtist2, StringSplitOptions.None);
                    if (ars2.Length > 1)
                        artists.AddRange(ars2);
                }

                List<string> artistsFullySplitted = new List<string>();
                string[] arsFull = parts[0].Split(sepArtistFull, StringSplitOptions.None);
                artistsFullySplitted.AddRange(arsFull);

                string song = parts[1];

                string keywords = "";
                //artists.ForEach(f => words += " " + f);
                keywords += artistsFullySplitted[0];
                if (artistsFullySplitted.Count > 1) keywords += " " + artistsFullySplitted[1];
                keywords += " " + song;

                string keywordsEncoded = Server.UrlEncode(keywords);

                Page<SpotifyWebAPI.Track> tracks = await SpotifyWebAPI.Track.SearchAll(keywordsEncoded);

                found = false;

                var state = new State { Artists = artists, Artist = artists[0], Song = song, Keywords = keywords, Playlist = playlist, Tracks = tracks };

                //if (CheckBoxPreFilter.Checked)
                //    tracks = FilterR(state, track => track.Artists[0].Name == artists[1] || (artists.Count > 2 && track.Artists[0].Name == artists[2]));

                if (CheckBoxPreFilter.Checked)
                    tracks = FilterR2(state);

                state = new State { Artists = artists, Artist = artists[0], Song = song, Keywords = keywords, Playlist = playlist, Tracks = tracks };

                //List(state, null);

                Header();

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name.Eq(song) && track.Album.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name.Eq(song) && track.Album.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);

                Filter(state, t => t.Artists[0].Name.Eq(artists[0]) && t.Name._StartsWith(song) && t.Name._NotContains(" mix", " remix") && t.Album.Name.Eq(song) && t.Album.AlbumType == AlbumType.Single);
                if (artists.Count > 1)
                Filter(state, t => t.Artists[0].Name.Eq(artists[1]) && t.Name._StartsWith(song) && t.Name._NotContains(" mix", " remix") && t.Album.Name.Eq(song) && t.Album.AlbumType == AlbumType.Single);

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Album);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Album);

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Album);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Album);

                if (artists.Count > 2)
                {
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name.Eq(song) && track.Album.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.Name.Eq(song) && track.Album.AlbumType == AlbumType.Single);
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Album);
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Album);
                }

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Compilation);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Compilation);

                Filter(state, track => track.Artists[0].Name.Eq(artists[0]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Compilation);
                if (artists.Count > 1)
                Filter(state, track => track.Artists[0].Name.Eq(artists[1]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Compilation);

                if (artists.Count > 2)
                {
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name.Eq(song) && track.Album.AlbumType == AlbumType.Compilation);
                    Filter(state, track => track.Artists[0].Name.Eq(artists[2]) && track.Name._StartsWith(song) && track.Name._NotContains(" mix", " remix") && track.Album.AlbumType == AlbumType.Compilation);
                }

                NotFound(state);
            }
        }

        private Page<SpotifyWebAPI.Track> FilterR(State State, Func<SpotifyWebAPI.Track, bool> Func)
        {
            Page<SpotifyWebAPI.Track> tracksReturn = new Page<SpotifyWebAPI.Track>();
            tracksReturn.Total = 0;

            foreach (SpotifyWebAPI.Track track in State.Tracks.Items)
            {
                if (Func.Invoke(track))
                {
                    tracksReturn.Items.Add(track);
                    tracksReturn.Total++;
                }
            }

            return tracksReturn;
        }

        //track => track.Artists[0].Name == artists[1] || (artists.Count > 2 && track.Artists[0].Name == artists[2])
        private Page<SpotifyWebAPI.Track> FilterR2(State State)
        {
            Page<SpotifyWebAPI.Track> tracksReturn = new Page<SpotifyWebAPI.Track>();
            tracksReturn.Total = 0;

            foreach (SpotifyWebAPI.Track track in State.Tracks.Items)
            {
                List<string> artistNames = new List<string>();
                foreach(SpotifyWebAPI.Artist a in track.Artists)
                {
                    artistNames.Add(a.Name);
                }

                bool foundArtist = false;
                foreach(string a in State.Artists)
                {
                    foundArtist = artistNames.Contains(a);
                    if (foundArtist) break;
                }

                if (foundArtist)
                {
                    tracksReturn.Items.Add(track);
                    tracksReturn.Total++;
                }
            }

            return tracksReturn;
        }

        private void List(State State, SpotifyWebAPI.Track Track)
        {
            System.Web.UI.WebControls.Table table = new Table();
            table.BorderWidth = 1;

            //Header
            TableRow row = new TableRow();
            row.Cells.Add(new TableCell { Text = "Song" });
            row.Cells.Add(new TableCell { Text = "Artist" });
            row.Cells.Add(new TableCell { Text = "Album name" });
            row.Cells.Add(new TableCell { Text = "Album type" });
            row.BackColor = System.Drawing.Color.Blue;
            row.ForeColor = System.Drawing.Color.White;
            table.Rows.Add(row);

            row = new TableRow();
            row.Cells.Add(new TableCell { Text = State.Song });
            row.Cells.Add(new TableCell { Text = State.Artist });
            row.Cells.Add(new TableCell { Text = "" });
            row.Cells.Add(new TableCell { Text = "" });
            row.BackColor = System.Drawing.Color.Orange;
            table.Rows.Add(row);

            foreach (SpotifyWebAPI.Track track in State.Tracks.Items)
            {
                    row = new TableRow();
                    //row.Cells.Add(new TableCell { Text = State.Artist });
                    //row.Cells.Add(new TableCell { Text = State.Song });
                    //row.Cells.Add(new TableCell { Text = State.Keywords });
                    row.Cells.Add(new TableCell { Text = track.Name });
                    string artists = "";
                    track.Artists.ForEach(a => artists += a.Name + ",");
                    artists = artists.Remove(artists.Length - 1);
                    row.Cells.Add(new TableCell { Text = artists });
                    row.Cells.Add(new TableCell { Text = track.Album.Name });
                    row.Cells.Add(new TableCell { Text = track.Album.AlbumType.ToString() });
                    if (track == Track) row.BackColor = System.Drawing.Color.Yellow;
                    table.Rows.Add(row);
            }

            TableRow rowTotal = new TableRow();
            rowTotal.Cells.Add(new TableCell { Text = State.Tracks.Total.ToString() });
            table.Rows.Add(rowTotal);

            TableDiv.Controls.Add(table);
            TableDiv.Controls.Add(new Label());
        }

        private void Header()
        {
            TableRow row = new TableRow();
            row.Cells.Add(new TableCell { Text = "State.Artist" });
            row.Cells.Add(new TableCell { Text = "State.Song" });
            row.Cells.Add(new TableCell { Text = "State.Keywords" });
            row.Cells.Add(new TableCell { Text = "track.Name" });
            //string artists = "";
            //track.Artists.ForEach(a => artists += a.Name + ",");
            //artists = artists.Remove(artists.Length - 1);
            row.Cells.Add(new TableCell { Text = "artists" });
            row.Cells.Add(new TableCell { Text = "track.Album.Name" });
            row.Cells.Add(new TableCell { Text = "track.Album.AlbumType" });
            Table.Rows.Add(row);
        }


        //private void Filter(SpotifyWebAPI.Playlist playlist, SpotifyWebAPI.Page<SpotifyWebAPI.Track> tracks, Func<SpotifyWebAPI.Track, bool> func)
        private void Filter(State State, Func<SpotifyWebAPI.Track, bool> Func)
        {
            if (!found)
            {
                foreach (SpotifyWebAPI.Track track in State.Tracks.Items)
                {
                    if (Func.Invoke(track))
                    {
                        found = true;
                        State.Playlist.AddTrack(track, this.AuthenticationToken);
                        TableRow row = new TableRow();
                        row.Cells.Add(new TableCell { Text=State.Artist });
                        row.Cells.Add(new TableCell { Text=State.Song });
                        row.Cells.Add(new TableCell { Text=State.Keywords });
                        row.Cells.Add(new TableCell { Text=track.Name });
                        string artists = "";
                        track.Artists.ForEach(a => artists += a.Name + ",");
                        artists = artists.Remove(artists.Length-1);
                        row.Cells.Add(new TableCell { Text=artists });
                        row.Cells.Add(new TableCell { Text = track.Album.Name });
                        row.Cells.Add(new TableCell { Text=track.Album.AlbumType.ToString() });
                        Table.Rows.Add(row);

                        List(State, track);
                        break;
                    }
                }
            }
        }

        private void NotFound(State State)
        {
            if (!found)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell { Text = State.Artist });
                row.Cells.Add(new TableCell { Text = State.Song });
                row.Cells.Add(new TableCell { Text = State.Keywords });
                row.Cells.Add(new TableCell { Text = "" });
                row.Cells.Add(new TableCell { Text = "" });
                row.Cells.Add(new TableCell { Text = "" });
                row.Cells.Add(new TableCell { Text = "" });
                Table.Rows.Add(row);
                
                TextBoxNotFound.Text += State.Artist + "\t" + State.Song + Environment.NewLine;

                List(State, null);
            }
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