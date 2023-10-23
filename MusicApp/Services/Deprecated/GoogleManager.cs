using Common;
using System.Net;

namespace Services
{
    public class GoogleManager
    {
        public static string FindURL(string artist, string title, string domain, string acceptLanguage)
        {
            return "";



            //string querystring = artist.Replace("+", "%2B").Replace(' ', '+').Replace("&", "%26") + "+" +
            //                     title.Replace("+", "%2B").Replace(' ', '+').Replace("&", "%26") + "+" +
            //                     domain.Replace("+", "%2B").Replace(' ', '+').Replace("&", "%26"); //"+lyrics";

            string querystring = artist + " " + title + " " + domain;
            querystring = WebUtility.UrlEncode(querystring);

            string url = "http://www.google.com/search?q=" + querystring;
            //string url = "https://www.google.com/search#q=" + querystring;

            var webclient = new GZipWebClient();

            webclient.Headers.Add(HttpRequestHeader.Host, "www.google.com");

            string content = webclient.DownloadString(url);

            var strSearch = new Str(content);

            strSearch.Find(" id=\"search\""); strSearch.Skip();

            while (strSearch.Find("<a "))
            {
                strSearch.Skip();
                strSearch.Find("href=\""); strSearch.Skip();
                string link = strSearch.Read("\"");

                if (link.Contains(domain))
                {
                    string newLink = link.Substring(link.IndexOf(domain));
                    link = link.Remove(link.IndexOf(domain));

                    int index = newLink.IndexOf("&");
                    if (index > -1)
                        newLink = newLink.Remove(index);

                    int lastIndex = link.LastIndexOf("http");
                    if (lastIndex > -1)
                        newLink = link.Substring(link.LastIndexOf("http")) + newLink;

                    return newLink.Replace("https://", "http://");
                }
            }

            return "";
        }

        public static string FindURL(string querystring, string domain, string acceptLanguage)
        {
            return "";



            querystring = WebUtility.UrlEncode(querystring + " site:" + domain);

            string url = "http://www.google.com/search?q=" + querystring;
            //string url = "https://www.google.com/search#q=" + querystring;

            var webclient = new GZipWebClient();

            webclient.Headers.Add(HttpRequestHeader.Host, "www.google.com");

            string content = webclient.DownloadString(url);

            var strSearch = new Str(content);

            strSearch.Find(" id=\"search\""); strSearch.Skip();

            while (strSearch.Find("<a "))
            {
                strSearch.Skip();
                strSearch.Find("href=\""); strSearch.Skip();
                string link = strSearch.Read("\"");

                if (link.Contains(domain))
                {
                    string newLink = link.Substring(link.IndexOf(domain));
                    link = link.Remove(link.IndexOf(domain));

                    int index = newLink.IndexOf("&");
                    if (index > -1)
                        newLink = newLink.Remove(index);

                    int lastIndex = link.LastIndexOf("http");
                    if (lastIndex > -1)
                        newLink = link.Substring(link.LastIndexOf("http")) + newLink;

                    return newLink.Replace("https://", "http://");
                }
            }

            return "";
        }
    }
}
