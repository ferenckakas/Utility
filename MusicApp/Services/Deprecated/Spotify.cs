using Common;
using Services.Chart;
using SpotifyWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class SpotifyOld
    {
        public static AuthenticationToken AuthenticationToken;

        private static AuthenticationToken GetAuth(Token token)
        {
            return new AuthenticationToken()
            {
                AccessToken = token.AccessToken,
                ExpiresOn = token.ExpiresOn,
                RefreshToken = token.RefreshToken,
                TokenType = token.TokenType,
                //Now = Session<DateTime>("Now"),
            };
        }

        public static async Task SavePlaylistsIntoTXT(Token token, string sCountry, string sChartName, string sTime)
        {
            string sGenre;
            sGenre = null;
            if (sGenre == null) sGenre = "All";

            if (sCountry != "custom")
                return;

           
            AuthenticationToken = GetAuth(token);

            // get the user
            User user = await User.GetCurrentUserProfile(AuthenticationToken);

            List<Playlist> playlists = await Playlist.GetAllPlaylists(user.Id, AuthenticationToken);

            foreach (Playlist playlist in playlists)
            {
                string TimeName = playlist.Name;

                CChart chart = null;
                List<CSong> list = null;
                bool bLatest = false;
                ChartInfo.GetInfo(sCountry, sChartName, sTime, TimeName, sGenre, ref chart, ref list, ref bLatest);

                List<PlaylistTrack> playlistTracks = await Playlist.GetAllPlaylistTracks(user.Id, playlist.Id, AuthenticationToken);

                List<CSong> list2 = new List<CSong>();
                foreach (PlaylistTrack playlistTrack in playlistTracks)
                {
                    CSong song = list.Where(s => s.SpotifyID == playlistTrack.Track.Id).FirstOrDefault();
                    if (song != null)
                    {
                        list2.Add(song);
                    }
                    else
                    {
                        string artist = String.Join(",", playlistTrack.Track.Artists.Select(a => a.Name).ToList());
                        string title = playlistTrack.Track.Name;

                        song = new CSong { Artist = artist, Title = title, VideoID = "", SpotifyID = playlistTrack.Track.Id };
                        list2.Add(song);
                    }
                }

                for (var i = list.Count - 1; i >= 0; i--)
                {
                    CSong song = list[i];

                    if (song.SpotifyID == "")
                    {
                        CSong prevSong = list.Previous(i, s => s.SpotifyID != "" && list2.Contains(s));

                        if (prevSong != null)
                            list2.Insert(list2.IndexOf(prevSong) + 1, song);
                        else
                            list2.Insert(0, song);
                    }
                }

                chart.Save(list2);
            }
        }

        public static async Task Post(Token token, string sCountry, string sChartName, string sTime)
        {
            //string sCountry = info.SelectedCountry;
            //string sChartName = info.SelectedChart;
            //string sTime = info.SelectedTime;
            string sGenre;
            sGenre = null;
            if (sGenre == null) sGenre = "All";

            if (sCountry != "custom")
                return;

            //     ChartInfo.LoadFromSession(); //kell?

            // string TimeName = ChartInfo.TimeList.Where(t => t.Id == sTime).First().Name;


            string TimeName = sTime;

            CChart chart = null;
            List<CSong> list = null;
            bool bLatest = false;
            ChartInfo.GetInfo(sCountry, sChartName, sTime, TimeName, sGenre, ref chart, ref list, ref bLatest);

            AuthenticationToken = GetAuth(token);

            // get the user
            User user = await User.GetCurrentUserProfile(AuthenticationToken);

            List<Playlist> playlists = await Playlist.GetAllPlaylists(user.Id, AuthenticationToken);

            Playlist playlist = playlists.Where(p => p.Name == chart.PlaylistName).FirstOrDefault();

            if (playlist == null)
            {

            }
            else
            {
                List<PlaylistTrack> playlistTracks = await Playlist.GetAllPlaylistTracks(user.Id, playlist.Id, AuthenticationToken);

                List<CSong> list2 = new List<CSong>();
                foreach (PlaylistTrack playlistTrack in playlistTracks)
                {
                    CSong song = list.Where(s => s.SpotifyID == playlistTrack.Track.Id).FirstOrDefault();
                    if (song != null)
                    {
                        list2.Add(song);
                    }
                    else
                    {
                        string artist = String.Join(",", playlistTrack.Track.Artists.Select(a => a.Name).ToList());
                        string title = playlistTrack.Track.Name;

                        song = new CSong { Artist = artist, Title = title, VideoID = "", SpotifyID = playlistTrack.Track.Id };
                        list2.Add(song);
                    }
                }

                for (var i = list.Count - 1; i >= 0; i--)
                {
                    CSong song = list[i];

                    if (song.SpotifyID == "")
                    {
                        CSong prevSong = list.Previous(i, s => s.SpotifyID != "" && list2.Contains(s));

                        if (prevSong != null)
                            list2.Insert(list2.IndexOf(prevSong) + 1, song);
                        else
                            list2.Insert(0, song);
                    }
                }

                chart.Save(list2);
            }
        }
    }
}
