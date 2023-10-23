using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Services.Chart
{
    public class UKRockSingles : CChart
    {
        public UKRockSingles()
        {
            _countryCode = "UK"; _country = "United Kingdom";
            _name = "Rock Singles";
            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            _chartFrequency = new TimeSpan(7, 21, 0, 0); //7 days 21 hours
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;
            
            _list = GetCachedChart();

            if (_list != null) return _list;
                        

            _list = new List<CSong>();

            string url = "http://www.bbc.co.uk/radio1/chart/rocksingles";

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

            DateTime Date = DateTime.Parse(_date);

            //_date = Date.ToShortDateString().Replace(" ", "");
            _date = Date.ToString("yyyy.MM.dd.");
            var writer = new StreamWriter(Path.Combine(ChartPath, _date + " (Latest).txt"));

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

                writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);

                _list.Add(song);
            }

            writer.Close();

            return _list;
        }
    }
}
