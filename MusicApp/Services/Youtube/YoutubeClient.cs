using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Services.AutoMapper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Youtube
{
    public class YoutubeClient
    {
        private YouTubeService _youtubeService;

        public List<Playlist> Playlists { get; set; }

        public async Task LoadPlaylists()
        {
            _youtubeService = await Authorization();
            Playlists = await _youtubeService.GetAllPlaylists();
        }

        public async Task<YouTubeService> Authorization()
        {
            UserCredential credential;
            using (var stream = new FileStream("Youtube\\client_data.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube },
                    "user", CancellationToken.None, new FileDataStore("Youtube.CreatePlaylist"));
            }

            return new YouTubeService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "MusicApp"
            });
        }

        public async Task<List<Common.Models.SearchResult>> SearchVideos(string term, int maxHit)
        {
            var bstr = new Bootstrapper();

            SearchResource.ListRequest searchListRequest = _youtubeService.Search.List("snippet");
            searchListRequest.Q = term;
            searchListRequest.Type = "video";
            searchListRequest.MaxResults = maxHit;

            // Call the search.list method to retrieve results matching the specified query term.
            SearchListResponse searchListResponse = await searchListRequest.ExecuteAsync();

            var searchResults = (List<SearchResult>)searchListResponse.Items;

            List<Common.Models.SearchResult> trackResults = Bootstrapper.Mapper.Map<List<SearchResult>, List<Common.Models.SearchResult>>(searchResults);

            return trackResults;
        }

        public async Task CreatePlaylist(string playlistName, List<string> newVideoIds)
        {
            //List<Playlist> playlists = await youtubeService.GetAllPlaylists();

            Playlist playlist = Playlists.Where(p => p.Snippet.Title == playlistName).FirstOrDefault();

            bool update = false;

            if (playlist == null)
            {
                // Create a new, private playlist in the authorized user's channel.
                playlist = new Playlist();
                playlist.Snippet = new PlaylistSnippet();
                playlist.Snippet.Title = playlistName;
                //playlist.Snippet.Description
                playlist.Status = new PlaylistStatus();
                playlist.Status.PrivacyStatus = "private";
                playlist = await _youtubeService.Playlists.Insert(playlist, "snippet,status").ExecuteAsync();
                Playlists.Add(playlist);
                update = true;
            }
            else
            {
                List<PlaylistItem> playlistItems = await _youtubeService.GetAllPlaylistItems(playlist.Id);

                if (playlistItems.Count == newVideoIds.Count)
                {
                    int i = 0;
                    foreach (PlaylistItem playlistItem in playlistItems)
                    {
                        if (playlistItem.ContentDetails.VideoId != newVideoIds[i])
                            break;
                        i++;
                    }

                    if (i < playlistItems.Count) update = true;
                }
                else
                    update = true;

                if (update)
                {
                    foreach (PlaylistItem playlistItem in playlistItems)
                    {
                        await _youtubeService.PlaylistItems.Delete(playlistItem.Id).ExecuteAsync();
                    }
                }
            }

            if (update)
            {
                foreach (string newVideoId in newVideoIds)
                {
                    // Add a video to the newly created playlist.
                    var newPlaylistItem = new PlaylistItem();
                    newPlaylistItem.Snippet = new PlaylistItemSnippet();
                    newPlaylistItem.Snippet.PlaylistId = playlist.Id;
                    newPlaylistItem.Snippet.ResourceId = new ResourceId();
                    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                    newPlaylistItem.Snippet.ResourceId.VideoId = newVideoId;
                    newPlaylistItem = await _youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
                }
            }
        }
    }
}
