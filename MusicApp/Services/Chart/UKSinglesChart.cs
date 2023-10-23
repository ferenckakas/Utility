using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Services.Chart
{
    public class UKSinglesChart : CChart
    {
        public UKSinglesChart()
        {
            _countryCode = "UK"; _country = "United Kingdom";
            //_name = "Singles Chart";
            _name = "UK Singles Chart";
            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            ChartPath = "";
            _chartFrequency = new TimeSpan(7, 21, 0, 0); //7 days 21 hours
        }
        
        public UKSinglesChart(string path)
        {
            ChartPath = path;
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;
            
            //list = GetCachedChart();
            _list = GetChartFromDB();

            if (_list != null) return _list;
                        

            _list = new List<CSong>();

            string url = "http://www.bbc.co.uk/radio1/chart/singles";

            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = webclient.DownloadString(url);

            var str = new Str(content);

            str.Find("cht-date"); str.Skip();
            str.Find(">"); str.Skip();
            _date = str.Read("<"); str.Skip();

            if (_date.Contains("st") && !_date.Contains("August")) _date = _date.Replace("st", "");
            if (_date.Contains("nd")) _date = _date.Replace("nd", "");
            if (_date.Contains("rd")) _date = _date.Replace("rd", "");
            if (_date.Contains("th")) _date = _date.Replace("th", "");

            Released = DateTime.Parse(_date);

            //Released = Date;

            //_date = Date.ToShortDateString().Replace(" ", "");
            _date = Released.ToString("yyyy.MM.dd.");
            //var writer = new StreamWriter(Path.Combine(sChartPath, _date + " (Latest).txt"));
            //var writer = new StreamWriter(Path.Combine(sChartPath, _date + "txt"));

            while (str.Find("cht-entry-wrapper"))
            {
                str.Skip();

                var song = new CSong();

                str.Find("cht-entry-position-wrapper"); str.Skip();
                str.Find("cht-entry-position"); str.Skip();
                str.Find(">"); str.Skip();
                song.Position = str.Read("<").Trim(); str.Skip();

                str.Find("cht-entry-title"); str.Skip();
                str.Find(">"); str.Skip();
                song.Title = str.Read("<").Trim().Replace("&amp;", "&"); str.Skip();

                str.Find("cht-entry-artist"); str.Skip();
                str.Find(">"); str.Skip();

                string s = str.Read("</div>").Trim();
                if (s.StartsWith("<"))
                {
                    var str2 = new Str(s);
                    str2.Find("<"); str2.Skip();
                    str2.Find(">"); str2.Skip();
                    song.Artist = str2.Read("<").Trim().Replace("&amp;", "&");
                }
                else
                    song.Artist = s.Replace("&amp;", "&");

                str.Skip();
                //song.Artist = str.Read("</div>").Trim().Replace("&amp;", "&"); str.Skip();

                str.Find("cht-entry-status"); str.Skip();
                str.Find(">"); str.Skip();
                song.Movement = str.Read("|").Trim(); str.Skip();
                int idx = song.Movement.IndexOf(' ');
                if (idx > -1)
                    song.Movement = song.Movement.Substring(0, idx); // "up 3"

                if (song.Movement == "non-mover") song.Movement = "non";

                ProcessArtist(ref song.Artist);
                ProcessTitle(ref song.Title);

                //writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);

                _list.Add(song);
            }

            SaveIntoDB(Released);


            ////old
            //str.Find("<h1>The Official UK Top 40 Singles Chart</h1>"); str.Skip();
            //str.Find("<h2 class=\"date\">"); str.Skip();
            //_date = str.Read("</h2>"); str.Skip();

            //DateTime date = DateTime.Parse(sDate);

            ////_date = date.ToShortDateString().Replace(" ", "");
            //_date = date.ToString("yyyy.MM.dd.");
            //var writer = new StreamWriter(Path.Combine(ChartPath, _date + "txt"));

            //while (str.Find("<span class=\"movement "))
            //{
            //    var song = new CSong();

            //    str.Find("<span class=\"movement "); str.Skip();
            //    song.Movement = str.Read("\">"); str.Skip();

            //    if (song.Movement == "non-mover") song.Movement = "non";

            //    str.Find("<span class=\"position\">"); str.Skip();
            //    song.Position = str.Read("</span>"); str.Skip();

            //    str.Find("<span class=\"artist\">"); str.Skip();
            //    song.Artist = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

            //    str.Find("<span class=\"track\">"); str.Skip();
            //    song.Title = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

            //    ProcessArtist(ref song.Artist);
            //    ProcessTitle(ref song.Title);

            //    writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);

            //    list.Add(song);
            //}
            //// old end


            //writer.Close();

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
            CChart chart = CChart.Instantiate("united-kingdom" + "/" + "singleschart" + "/" + "");
            //chart = new UKSinglesChartNo1s();
            //string year = _time.Split('-')[3];
            //list = chart.GetChart(sYear + ".");
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


        public List<CSong> SaveIntoDB(string fileName)
        {
            var reader = new StreamReader(fileName); // System.Text.Encoding.Default?

            if (reader == null) return null;

            _list = new List<CSong>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');

                var song = new CSong();

                song.Position = fields[0];
                song.Movement = fields[1];
                song.Artist = fields[2];
                song.Title = fields[3];
                if (fields.Length > 4)
                    song.ImageUrl = fields[4]; // new

                //if (fields.Length > 4)
                //    song.VideoID = fields[4];
                //else
                //    song.VideoID = "";

                //if (fields.Length > 5)
                //    song.ImageUrl = fields[5];
                //else
                //    song.ImageUrl = "";

                song.Date = "";

                _list.Add(song);
            }

            reader.Close();

            return _list;
        }
    }
}
