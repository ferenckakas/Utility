using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Services.Chart
{
    public class ITunesChart : CChart
    {
        private readonly string _genre;

        public ITunesChart(string countryCode, string country, string genre)
        {
            _countryCode = countryCode;
            _country = country;
            _genre = genre;

            if (genre == "All")
                _name = "iTunes Top 100 Hits";
            else
                _name = "iTunes Top 100 " + genre + " Hits";

            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _countryCode.ToUpper() + @"\iTunes\" + _genre;
            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\iTunes Chart";
            _chartFrequency = new TimeSpan(1, 0, 0, 0); //1 day
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;


            _list = new List<CSong>();

            StreamWriter writer = null;

            string url;
            if (_genre == "All")
                url = "http://itunes.apple.com/" + _countryCode.ToLower() + "/rss/topsongs/limit=100/explicit=true/xml";
            else
                url = "http://itunes.apple.com/" + _countryCode.ToLower() + "/rss/topsongs/limit=100/explicit=true/xml";

            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = webclient.DownloadString(url);

            var str = new Str(content);

            /*str.Find(""); str.Skip();
            string datestr = str.Read("*</b>"); str.Skip();

            DateTime date = DateTime.Parse(datestr);*/

            str.Find("<updated"); str.Skip();
            str.Find(">"); str.Skip();
            _date = str.Read("</updated>"); str.Skip();

            DateTime date = DateTime.Parse(_date);

            //_date = date.ToShortDateString().Replace(" ", "");
            _date = date.ToString("yyyy.MM.dd.");
            writer = new StreamWriter(Path.Combine(ChartPath, _date + " (Latest).txt"));

            int position = 0;

            while (str.Find("<entry>"))
            {
                var song = new CSong();

                position++;

                song.Position = position.ToString();

                str.Find("<im:name"); str.Skip();
                str.Find(">"); str.Skip();
                song.Title = str.Read("</im:name>").Replace("&amp;", "&"); str.Skip();

                str.Find("<im:artist"); str.Skip();
                str.Find(">"); str.Skip();
                song.Artist = str.Read("</im:artist>").Replace("&amp;", "&"); str.Skip();

                str.Find("<im:image"); str.Skip();
                str.Find("height=\"60\""); str.Skip();
                str.Find(">"); str.Skip();
                song.ImageUrl = str.Read("</im:image>"); str.Skip();

                song.Movement = "non";

                ProcessArtist(ref song.Artist);
                ProcessTitle(ref song.Title);

                song.Artist = HttpUtility.HtmlDecode(song.Artist);
                song.Title = HttpUtility.HtmlDecode(song.Title);

                writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title + "\t" + song.ImageUrl);

                _list.Add(song);
            }

            writer.Close();

            return _list;
        }
    }
}
