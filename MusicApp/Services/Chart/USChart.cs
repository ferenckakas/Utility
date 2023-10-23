using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Services.Chart
{
    public class USChart : CChart
    {
        public USChart()
        {
            _countryCode = "US"; _country = "United States";
            _name = "Billboard Hot 100";
            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            _chartFrequency = new TimeSpan(7, 21, 0, 0); //7 days 21 hours
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;


            _list = new List<CSong>();

            StreamWriter writer = null;

            for (var begin = 1; begin < 100; begin += 10)
            {
                string baseUrl = "http://m.billboard.com/charts/hot-100";
                string url;

                if (begin == 1)
                {
                    url = baseUrl;
                }
                else
                {
                    url = baseUrl + "?begin=" + begin.ToString() + "&order=position";
                }

                var webclient = new WebClient();

                string content = webclient.DownloadString(url);

                var str = new Str(content);

                /*str.Find(""); str.Skip();
                string datestr = str.Read("*</b>"); str.Skip();

                DateTime date = DateTime.Parse(datestr);*/

                if (begin == 1)
                {
                    DateTime date = DateTime.Now;

                    while (date.DayOfWeek != DayOfWeek.Thursday)
                    {
                        date = date.AddDays(-1);
                    }

                    //_date = date.ToShortDateString().Replace(" ", "");
                    _date = date.ToString("yyyy.MM.dd.");
                    writer = new StreamWriter(Path.Combine(ChartPath, _date + " (Latest).txt"));
                }

                while (str.Find("<div class=\"chartPinkBox\"><b>"))
                {
                    var song = new CSong();

                    str.Find("<div class=\"chartPinkBox\"><b>"); str.Skip();
                    song.Position = str.Read("</b>").Replace("*", ""); str.Skip();

                    str.Find("<span style=\"font-size: 12px;\">"); str.Skip();
                    song.Title = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

                    str.Find("<span style=\"font-size: 10px;\">"); str.Skip();
                    song.Artist = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

                    str.Find("Prev Week:"); str.Skip();
                    string prevWeek = str.Read("&"); str.Skip();

                    if (prevWeek == "-")
                    {
                        song.Movement = "new";
                    }
                    else
                    {
                        int position = Convert.ToInt32(song.Position);
                        int prevWeekInt = Convert.ToInt32(prevWeek);

                        if (position < prevWeekInt)
                            song.Movement = "up";
                        else if (position > prevWeekInt)
                            song.Movement = "down";
                        else //=
                            song.Movement = "non";
                    }

                    ProcessArtist(ref song.Artist);
                    ProcessTitle(ref song.Title);

                    writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);

                    _list.Add(song);
                }
                var r = new Random(begin);
                int milliSec = r.Next(1000, 5000);
                Thread.Sleep(milliSec);
            }

            writer.Close();

            return _list;
        }

        public void GetNo1Charts(int from, int to)
        {
            ChartPath = ChartPath + @"\No1s";

            for (int i = from; i <= to; i++)
            {
                GetNoChart(i);
            }
        }

        public List<CSong> GetNoChart(int year)
        {
            if (year < 1940 || year > 2012)
                throw new Exception("Invalid Year.");

            string url = "http://en.wikipedia.org/wiki/List_of_Hot_100_number-one_singles_of_" + year.ToString() + "_(U.S.)";

            //if (list != null) return list;

            _list = new List<CSong>();

            var writer = new StreamWriter(Path.Combine(ChartPath, year.ToString() + ".txt"), false, System.Text.Encoding.UTF8);

            var webclient = new WebClient();

            webclient.Headers.Add(HttpRequestHeader.Host, "en.wikipedia.org");
            //webclient.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
            //webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1");
            webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            //webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            webclient.Headers.Add(HttpRequestHeader.Accept, "text/html, application/xhtml+xml, */*");
            //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            //webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
            webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en");
            //webclient.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");

            string content;
            try
            {
                content = webclient.DownloadString(url);
            }
            catch(Exception)
            {
                writer.Close();
                return _list;
            }

            //string title = "";
            //string artist = "";

            var str = new Str(content);

            str.Find("<table class=\"wikitable\">"); str.Skip();
            str.Find("<tr>"); str.Skip();

            int index = str.IndexOf("</table>");

            while (str.Find("<tr>") && str.Pos < index)
            {
                str.Skip();

                var song = new CSong();

                str.Find("<td"); str.Skip();
                str.Find(">"); str.Skip();
                song.ReachedNo1 = str.Read("</td>"); str.Skip();

                str.Find("<td "); str.Skip();
                string s = str.Read(">"); str.Skip();
                var td = new Str(s);
                if (td.Found("rowspan="))
                {
                    td.Find("rowspan=\""); td.Skip();
                    song.WeeksAtNo1 = td.Read("\""); td.Skip();
                }
                else
                    song.WeeksAtNo1 = "1";

                song.Title = str.Read("</td>"); str.Skip();

                if (song.Title == "" || song.Title.StartsWith("<sup")) continue;

                if (song.Title.IndexOf("\"") == 0)
                    song.Title = song.Title.Remove(0, 1);

                if (song.Title.LastIndexOf("\"") > -1)
                    song.Title = song.Title.Substring(0, song.Title.LastIndexOf("\""));


                //if (song.Title.StartsWith("<sup"))
                //{
                //    song.Title = Title;
                //    song.Artist = Artist;
                //    list.Add(song);
                //    continue;
                //}

                //Title = song.Title;

                str.Find("<td"); str.Skip();
                str.Find(">"); str.Skip();
                song.Artist = str.Read("</td>"); str.Skip();
                //Artist = str.Read("</td>"); str.Skip();
                //song.Artist = Artist;

                //Artist = song.Artist;

                writer.WriteLine(song.ReachedNo1 + "\t" + song.WeeksAtNo1 + "\t" + song.Artist + "\t" + song.Title);

                _list.Add(song);
            }

            writer.Close();

            return _list;
        }

        public void CleanNo1Charts(int from, int to)
        {
            ChartPath = ChartPath + @"\No1s_cleaned";

            for (int i = from; i <= to; i++)
            {
                CleanChart(i);
            }
        }

        public List<CSong> CleanChart(int year)
        {
            CChart chart = CChart.Instantiate("united-states" + "/" + "" + "/" + "");
            //chart = new UKSinglesChartNo1s();
            //string year = _time.Split('-')[3];
            //list = chart.GetChart(year + ".");
            List<CSong> list = chart.GetNo1Chart(year.ToString() + ".");

            var writer = new StreamWriter(Path.Combine(ChartPath, year.ToString() + ".txt"), false, System.Text.Encoding.UTF8);

            foreach (CSong song in list)
            {
                string artist = "";
                string title = "";

                var str = new Str(song.Artist);

                if (str.Found("<a"))
                {
                    artist = "";
                    while (str.Find("<a"))
                    {
                        str.Skip();
                        str.Find(">"); str.Skip();
                        artist += str.Read("</a>");
                        str.Find("</a>"); str.Skip();
                        if (str.Found("<"))
                        {
                            artist += str.Read("<");
                        }
                        else
                        {
                            artist += str.ReadToEnd();
                        }
                    }
                }
                else
                {
                    artist = song.Artist;
                }

                str = new Str(song.Title);

                if (str.Found("<a"))
                {
                    title = "";
                    while (str.Find("<a"))
                    {
                        str.Skip();
                        str.Find(">"); str.Skip();
                        title += str.Read("</a>");
                        str.Find("</a>"); str.Skip();
                        if (str.Found("<"))
                        {
                            title += str.Read("<");
                        }
                        else
                        {
                            title += str.ReadToEnd();
                        }
                    }
                }
                else
                {
                    title = song.Title;
                }

                song.Artist = artist;
                song.Title = title;

                writer.WriteLine(song.ReachedNo1 + "\t" + song.WeeksAtNo1 + "\t" + song.Artist + "\t" + song.Title);
            }

            writer.Close();

            return list;
        }
    }
}
