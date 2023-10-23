using Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Services.Chart;
using Services.Old;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Services.Youtube.Mvc
{
    public class YoutubeManager
    {
        //private YouTubeService _youtubeService;

        public List<Playlist> Playlists { get; set; }

        //private YouTubeResultItem _youtubeResultItem;

        //List<Common.Models.SearchResult> SearchResults;
        //SongTitle CurrentSong;
        private readonly List<string> _newVideoIds = new List<string>();

        public IConfigurableHttpClientInitializer GetCredential()
        {
            Application.Trace("YouTube API", "YouTube.GetCredential: Begin");

            IConfigurableHttpClientInitializer httpClientInitializer;

            if (Application.DBSession())
            {
                using (var entities = new Entities.MusicEntities())
                {
                    string token = entities.Session("Token");
                    string userId = entities.Session("UserId");

                    if (token != null && userId != null)
                    {
                        var jsSerializer = new JavaScriptSerializer();
                        var Token = jsSerializer.Deserialize<TokenResponse>(token);
                        string UserId = jsSerializer.Deserialize<string>(userId);

                        httpClientInitializer = new UserCredential(new AppFlowMetadata().Flow, UserId, Token);
                        Application.Trace("YouTube API", "YouTube.GetCredential: httpClientInitializer = new UserCredential(new AppFlowMetadata().Flow, UserId, Token);");
                    }
                    else
                    {
                        httpClientInitializer = null;
                        Application.Trace("YouTube API", "httpClientInitializer = null;");
                    }
                }
            }
            //else
            //{
            //    if (HttpContext.Current.Session["Credential"] != null)
            //        httpClientInitializer = (IConfigurableHttpClientInitializer)HttpContext.Current.Session["Credential"];
            //    else
            //        httpClientInitializer = null;
            //}
            else
                httpClientInitializer = null;

            return httpClientInitializer;
        }

        public async Task Post(string country, string chartName, string time)
        {
            //string country = info.SelectedCountry;
            //string chartName = info.SelectedChart;
            //string time = info.SelectedTime;
            string genre;
            genre = null;
            if (genre == null) genre = "All";

            if (country != "custom")
                return;

            //    ChartInfo.LoadFromSession(); //kell?

            // string timeName = ChartInfo.TimeList.Where(t => t.Id == sTime).First().Name;

            string timeName = time;

            CChart chart = null;
            List<CSong> list = null;
            bool latest = false;
            ChartInfo.GetInfo(country, chartName, time, timeName, genre, ref chart, ref list, ref latest);

            IConfigurableHttpClientInitializer httpClientInitializer = GetCredential();

            if (httpClientInitializer != null)
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = httpClientInitializer,
                    ApplicationName = "MusicApp" //GetType().ToString()
                });

                List<Playlist> playlists = await youtubeService.GetAllPlaylists();

                Playlist playlist = playlists.Where(p => p.Snippet.Title == chart.PlaylistName).FirstOrDefault();

                if (playlist == null)
                {
                   // ((CustomChart)chart).SaveNotFound(playlists);
                }
                else
                {
                  //  ((CustomChart)chart).SaveFound(playlists);

                    List<PlaylistItem> playlistItems = await youtubeService.GetAllPlaylistItems(playlist.Id);

                    var list2 = new List<CSong>();
                    foreach (PlaylistItem playlistItem in playlistItems)
                    {
                        CSong song = list.Where(s => s.VideoID == playlistItem.ContentDetails.VideoId).FirstOrDefault();
                        if (song != null)
                        {
                            list2.Add(song);
                        }
                        else
                        {
                            string artist = TitleParser.GetArtist(playlistItem.Snippet.Title);
                            string title = TitleParser.GetTitle(playlistItem.Snippet.Title);

                            song = new CSong { Artist = artist, Title = title, VideoID = playlistItem.ContentDetails.VideoId, SpotifyID = "" };
                            list2.Add(song);
                        }
                    }

                    for (var i = list.Count - 1; i >= 0; i--)
                    {
                        CSong song = list[i];

                        if (song.VideoID == "")
                        {
                            CSong prevSong = list.Previous(i, s => s.VideoID != "" && list2.Contains(s));

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
}
