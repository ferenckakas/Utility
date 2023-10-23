using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Youtube
{
    public static class YouTubeServiceExtenstion
    {
        public static async Task<List<Playlist>> GetAllPlaylists(this YouTubeService youtubeService)
        {
            var playlists = new List<Playlist>();

            string nextPageToken = "";
            while (nextPageToken != null)
            {
                PlaylistsResource.ListRequest playlistsRequest = youtubeService.Playlists.List("snippet");
                playlistsRequest.Mine = true;
                playlistsRequest.MaxResults = 50;
                playlistsRequest.PageToken = nextPageToken;

                PlaylistListResponse playlistsResponse = await playlistsRequest.ExecuteAsync();
                foreach (Playlist p in playlistsResponse.Items)
                {
                    playlists.Add(p);
                }

                nextPageToken = playlistsResponse.NextPageToken;
            }

            return playlists;
        }

        public static async Task<List<PlaylistItem>> GetAllPlaylistItems(this YouTubeService youtubeService, string playlistId)
        {
            var playlistItems = new List<PlaylistItem>();

            string nextPageToken = "";
            while (nextPageToken != null)
            {
                PlaylistItemsResource.ListRequest playlistItemsRequest = youtubeService.PlaylistItems.List("snippet,contentDetails");
                playlistItemsRequest.PlaylistId = playlistId;
                playlistItemsRequest.MaxResults = 50;
                playlistItemsRequest.PageToken = nextPageToken;

                PlaylistItemListResponse playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();

                foreach (PlaylistItem playlistItem in playlistItemsResponse.Items)
                {
                    playlistItems.Add(playlistItem);
                }

                nextPageToken = playlistItemsResponse.NextPageToken;
            }

            return playlistItems;
        }
    }
}
