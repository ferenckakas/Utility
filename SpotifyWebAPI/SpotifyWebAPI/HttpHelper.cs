using SpotifyWebAPI.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.Services.WebApi;

namespace SpotifyWebAPI
{
    /// <summary>
    /// A helper class used as an interface for common HttpClient commands
    /// </summary>
    internal class HttpHelper
    {
        /// <summary>
        /// Downloads a url and reads its contents as a string using the get method
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> Get(string url)
        {
            var client = new HttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync(url);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Downloads a url and reads its contents as a string, requires an authorization token
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> Get(string url, AuthenticationToken token, bool includeBearer = true)
        {
            var client = new HttpClient();
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.GetAsync(url);

            if ((int)httpResponse.StatusCode == 429)
            {
                int secs = httpResponse.Headers.RetryAfter.Delta.HasValue ? httpResponse.Headers.RetryAfter.Delta.Value.Seconds : 2;
                throw new TooManySpotifyRequestException(secs);
            }

            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// posts data to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static async Task<string> Post(string url, Dictionary<string, string> postData = null)
        {
            HttpContent content = null;
            if (postData != null)
                content = new FormUrlEncodedContent(postData.ToArray<KeyValuePair<string, string>>());
            else
                content = null;

            var client = new HttpClient();

            HttpResponseMessage httpResponse = await client.PostAsync(url, content);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// posts data to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="token"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Post(string url, AuthenticationToken token, Dictionary<string, string> postData = null, bool includeBearer = true)
        {
            HttpContent content = null;
            if (postData != null)
                content = new FormUrlEncodedContent(postData.ToArray<KeyValuePair<string, string>>());
            else
                content = null;

            var client = new HttpClient();
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.PostAsync(url, content);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// posts data to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="token"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Post(string url, AuthenticationToken token, string jsonString, bool includeBearer = true)
        {
            HttpContent content = new StringContent(jsonString);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.PostAsync(url, content);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// put data to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="token"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Put(string url, AuthenticationToken token, Dictionary<string, string> postData = null, bool includeBearer = true)
        {
            HttpContent content = null;

            if (postData != null)
                content = new FormUrlEncodedContent(postData.ToArray<KeyValuePair<string, string>>());
            else
                content = null;

            var client = new HttpClient();
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.PutAsync(url, content);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// put data to a url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="token"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Put(string url, AuthenticationToken token, string jsonString, bool includeBearer = true)
        {
            HttpContent content = new StringContent(jsonString);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.PutAsync(url, content);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// http delete command
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Delete(string url, AuthenticationToken token, bool includeBearer = true)
        {
            var client = new HttpClient();
            if (includeBearer)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            else
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

            HttpResponseMessage httpResponse = await client.DeleteAsync(url);
            return await httpResponse.Content.ReadAsStringAsync();
        }

        ///// <summary>
        ///// http delete command
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="token"></param>
        ///// <param name="jsonString"></param>
        ///// <param name="includeBearer"></param>
        ///// <returns></returns>
        //public static async Task<string> Delete(string url, AuthenticationToken token, string jsonString, bool includeBearer = true)
        //{
        //    HttpContent content = new StringContent(jsonString);

        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    if (includeBearer)
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        //    else
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.AccessToken);

        //    HttpResponseMessage httpResponse = await client.DeleteAsync(url, /*content*/);
        //    return await httpResponse.Content.ReadAsStringAsync();
        //}

        /// <summary>
        /// http delete command
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="jsonString"></param>
        /// <param name="includeBearer"></param>
        /// <returns></returns>
        public static async Task<string> Delete(string url, AuthenticationToken token, string jsonString, bool includeBearer = true)
        {
            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {token.AccessToken}");

            httpRequestMessage.Content = new StringContent(jsonString);

            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<string> Send(HttpMethod method, string url, string accessToken, string jsonString = null)
        {
            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage(method, url);

            httpRequestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");

            if (jsonString != null)
                httpRequestMessage.Content = new StringContent(jsonString);

            HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return null;

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public static async Task<string> Post(string url, string accessToken, string jsonString = null)
        {
            return await Send(HttpMethod.Post, url, accessToken, jsonString);
        }

        public static async Task<string> Delete(string url, string accessToken, string jsonString = null)
        {
            return await Send(HttpMethod.Delete, url, accessToken, jsonString);
        }
    }
}
