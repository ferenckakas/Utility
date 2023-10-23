using Newtonsoft.Json;
using SpotifyWebAPI.SpotifyModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyWebAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class Authentication : BaseModel
    {
        public static string ClientId { get; set; }

        public static string ClientSecret { get; set; }

        public static string RedirectUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async static Task<AuthenticationToken> GetAccessToken(string code)
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                return null;
            }

            var postData = new Dictionary<string, string>();
            postData.Add("grant_type", "authorization_code");
            postData.Add("code", code);
            postData.Add("redirect_uri", RedirectUri);
            postData.Add("client_id", ClientId);
            postData.Add("client_secret", ClientSecret);

            string json = await HttpHelper.Post("https://accounts.spotify.com/api/token", postData);
            var obj = JsonConvert.DeserializeObject<accesstoken>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });

            return obj.ToPOCO();
        }
    }
}
