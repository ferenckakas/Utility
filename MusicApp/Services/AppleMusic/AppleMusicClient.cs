using Common;
using Newtonsoft.Json;
using Services.AppleMusic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Services.AppleMusic
{
    public class AppleMusicClient
    {
        private const string _baseUrl = "https://api.music.apple.com";

        private readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });

        private static readonly string _developerToken;
        private static readonly string _musicUserToken;

        static AppleMusicClient()
        {
            _developerToken = File.ReadAllText(Path.Combine(Constant.MediaLibraryConfigDirectory, "apple-developer-token.txt"));
            _musicUserToken = File.ReadAllText(Path.Combine(Constant.MediaLibraryConfigDirectory, "apple-music-user-token.txt"));
        }

        public async Task<List<Playlist>> GetAllPlaylists()
        {
            var playlists = new List<Playlist>();

            string url = $"/v1/me/library/playlists?limit=100";

            while (!string.IsNullOrEmpty(url))
            {
                PlaylistsResponse playlistsResponse = await GetAllPlaylistsResponse($"{_baseUrl}{url}");

                if (playlistsResponse == null)
                    break;

                foreach (Playlist playlist in playlistsResponse.Data)
                {
                    playlists.Add(playlist);
                }

                url = playlistsResponse.Next;
            }

            return playlists;
        }

        private async Task<PlaylistsResponse> GetAllPlaylistsResponse(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {_developerToken}");
            httpRequestMessage.Headers.Add("Music-User-Token", _musicUserToken);

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PlaylistsResponse>(content);
        }


        public async Task<List<Song>> GetSongs(string playlistId)
        {
            var songs = new List<Song>();

            string url = $"/v1/me/library/playlists/{playlistId}/tracks";

            while (!string.IsNullOrEmpty(url))
            {
                SongsResponse songsResponse = await GetSongsResponse($"{_baseUrl}{url}"); //limit=100

                if (songsResponse == null)
                    break;

                foreach (Song song in songsResponse.Data)
                {
                    songs.Add(song);
                }

                url = songsResponse.Next;
            }

            return songs;
        }

        private async Task<SongsResponse> GetSongsResponse(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {_developerToken}");
            httpRequestMessage.Headers.Add("Music-User-Token", _musicUserToken);

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SongsResponse>(content);
        }


        public async Task<List<Song>> SearchSongs(string term, int limit, int total)
        {
            var songs = new List<Song>();

            string url = $"/v1/catalog/hu/search?term={HttpUtility.UrlEncode(term)}&limit={limit}&types=songs";

            while (!string.IsNullOrEmpty(url))
            {
                SearchSongsResponse songsResponse = await SearchSongsResponse($"{_baseUrl}{url}"); //limit=100

                if (songsResponse?.Results?.Songs == null)
                    break;

                foreach (Song song in songsResponse.Results.Songs.Data)
                {
                    songs.Add(song);
                    if (songs.Count >= total)
                        break;
                }

                if (songs.Count >= total)
                    break;

                url = songsResponse.Results.Songs.Next;
            }

            return songs;
        }

        private async Task<SearchSongsResponse> SearchSongsResponse(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {_developerToken}");

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SearchSongsResponse>(content);
        }


        public async Task<Playlist> CreatePlaylist(string playlistName, List<string> songIds = null)
        {
            string url = $"{_baseUrl}/v1/me/library/playlists";

            var requestBody = new LibraryPlaylistCreationRequest
            {
                Attributes = new LibraryPlaylistCreationRequest_Attributes
                {
                    Name = playlistName,
                    Description = $"{playlistName} Description"
                }
            };

            if (songIds != null)
            {
                requestBody.Relationships = new LibraryPlaylistCreationRequest_Relationships
                {
                    Tracks = new LibraryPlaylistCreationRequest_RelationshipsTracks
                    {
                        Data = new List<LibraryPlaylistRequestTrack>()
                    }
                };

                for (var i = 0; i < Math.Min(songIds.Count, 25); i++)
                {
                    string id = songIds[i];

                    requestBody.Relationships.Tracks.Data.Add(
                        new LibraryPlaylistRequestTrack
                        {
                            Id = id,
                            Type = "songs"
                        }
                    );
                }
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {_developerToken}");
            httpRequestMessage.Headers.Add("Music-User-Token", _musicUserToken);

            string body = JsonConvert.SerializeObject(requestBody);

            httpRequestMessage.Content = new StringContent(body);

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<LibraryPlaylistResponse>(content);

            if (response?.Data == null || response?.Data.Count < 1)
                return null;


            string playlistId = response.Data[0].Id;

            while (songIds.Count > 25)
            {
                songIds.RemoveRange(0, 25);

                bool success = await AddTracksToPlaylist(playlistId, songIds);

                if (!success) break;
            }


            return response.Data[0];
        }

        public async Task<bool> AddTracksToPlaylist(string playlistId, List<string> songIds)
        {
            if (songIds == null) return false;

            string url = $"{_baseUrl}/v1/me/library/playlists/{playlistId}/tracks";

            var requestBody = new LibraryPlaylistCreationRequest_RelationshipsTracks
            {
                Data = new List<LibraryPlaylistRequestTrack>()
            };

            for (var i = 0; i < Math.Min(songIds.Count, 25); i++)
            {
                string id = songIds[i];

                requestBody.Data.Add(
                    new LibraryPlaylistRequestTrack
                    {
                        Id = id,
                        Type = "songs"
                    }
                );
            }

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {_developerToken}");
            httpRequestMessage.Headers.Add("Music-User-Token", _musicUserToken);

            string body = JsonConvert.SerializeObject(requestBody);

            httpRequestMessage.Content = new StringContent(body);

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            return httpResponseMessage.IsSuccessStatusCode;
        }

        public int[] Method(int[] a, int b)
        {

            for (var i = 0; i < a.Length; i++)
            {
                for (var j = i + 1; j < a.Length; j++)
                {
                    if (a[i] + a[j] == b)
                        return new int[] { i, j };
                }

            }

            return null;
        }
    }
}
