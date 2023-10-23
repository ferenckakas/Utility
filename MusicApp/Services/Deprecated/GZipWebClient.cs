using System;
using System.Net;

namespace Services
{
    public class GZipWebClient : WebClient
    {
        public GZipWebClient() : base()
        {
            Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch, br");
            Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8,hu;q=0.6");
            //Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            //Headers.Add(HttpRequestHeader.Connection, "keep-alive");            
            Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36");
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }
}
