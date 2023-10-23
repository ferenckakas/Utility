using Common;
using Newtonsoft.Json;
using Services.Discogs.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Discogs
{
    public class DiscogsClient
    {
        private readonly string _baseUrl = "https://api.discogs.com";

        private readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });

        private static readonly string _personalAccessToken;

        static DiscogsClient()
        {
            _personalAccessToken = File.ReadAllText(Path.Combine(Constant.MediaLibraryConfigDirectory, "discogs-personal-access-token.txt"));
        }

        public async Task<List<Result>> Search(string artist, string name, int total, bool withArtist)
        {
            var results = new List<Result>();

            string url;
            if (withArtist)
                url = $"{_baseUrl}/database/search?type=master&artist={WebUtility.UrlEncode(artist)}&query={WebUtility.UrlEncode(name)}&per_page=100&page=1";
            else
                url = $"{_baseUrl}/database/search?type=master&query={WebUtility.UrlEncode(artist)}+{WebUtility.UrlEncode(name)}&per_page=100&page=1";
            // track params has an apostrophy issue (no relevant hits)

            while (!string.IsNullOrEmpty(url))
            {
                Thread.Sleep(1100);

                SearchResponse songsResponse = await SearchResponse($"{url}"); //limit=100

                if (songsResponse?.Results == null || songsResponse?.Results?.Count == 0)
                    break;

                results.AddRange(songsResponse.Results);

                //foreach (Result result in songsResponse.Results)
                //{
                //    results.Add(result);
                //    //if (results.Count >= total)
                //    //    break;
                //}

                //if (results.Count >= total)
                //    break;

                url = songsResponse.Pagination.Urls.Next;
            }

            //var results2 = new List<Result>();

            //int c1 = results.Count(r => r.Format.Contains("Vinyl"));
            //int c2 = results.Count(r => r.Format.Contains("CD"));

            //results2.AddRange(results.Where(r => r.Format._ContainsSubstring("single")));

            //results2.AddRange(results.Where(r => r.Country == "US"));
            //results2.AddRange(results.Where(r => r.Country == "UK"));
            //results2.AddRange(results.Where(r => r.Country == "Germany"));
            //results2.AddRange(results.Where(r => r.Country == "France"));
            //results2.AddRange(results.Where(r => r.Country == "Hungary"));

            return results;
        }

        private async Task<SearchResponse> SearchResponse(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Add("User-Agent", $"LibraryUtilityTest/0.1");
            httpRequestMessage.Headers.Add("Authorization", $"Discogs token={_personalAccessToken}");

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SearchResponse>(content);
        }

        public async Task<List<Release>> GetReleasesByLabel(int labelId)
        {
            var releases = new List<Release>();

            string url = $"{_baseUrl}/labels/{labelId}/releases";

            while (!string.IsNullOrEmpty(url))
            {
                Thread.Sleep(1100);

                AllLabelReleasesResponse allLabelReleasesResponse = await GetReleasesByLabelResponse($"{url}"); //limit=100

                if (allLabelReleasesResponse?.Releases == null || allLabelReleasesResponse?.Releases?.Count == 0)
                    break;

                releases.AddRange(allLabelReleasesResponse.Releases);

                url = allLabelReleasesResponse.Pagination.Urls.Next;
            }

            return releases;
        }

        private async Task<AllLabelReleasesResponse> GetReleasesByLabelResponse(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            httpRequestMessage.Headers.Add("User-Agent", $"LibraryUtilityTest/0.1");
            httpRequestMessage.Headers.Add("Authorization", $"Discogs token={_personalAccessToken}");

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AllLabelReleasesResponse>(content);
        }

        public async Task<ReleaseResponse> GetRelease(string resourceUrl)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, resourceUrl);

            httpRequestMessage.Headers.Add("User-Agent", $"LibraryUtilityTest/0.1");
            httpRequestMessage.Headers.Add("Authorization", $"Discogs token={_personalAccessToken}");

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ReleaseResponse>(content);
        }

        public async Task<ReleaseResponse> GetRelease(int releaseId)
        {
            string url = $"{_baseUrl}/releases/{releaseId}";

            return await GetRelease(url);
        }
    }
}
