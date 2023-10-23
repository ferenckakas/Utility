using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Services.Chart
{
    public class HUVIVAChart : CChart
    {
        public HUVIVAChart()
        {
            _countryCode = "HU"; _country = "Hungary";
            _name = "VIVA Chart";
            //sChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            _chartFrequency = new TimeSpan(7, 21, 0, 0); //7 days 21 hours
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;


            _list = new List<CSong>();

            StreamWriter writer = null;

            for (var begin = 0; begin < 40; begin += 10)
            {
                string baseUrl = "http://www.vivatv.hu/charts/viva-chart/";
                string url;

                if (begin == 0)
                {
                    url = baseUrl;
                }
                else
                {
                    url = baseUrl + "?start=" + begin.ToString();
                }

                var webclient = new WebClient();
                webclient.Encoding = Encoding.UTF8;

                string content = webclient.DownloadString(url);

                var str = new Str(content);

                if (begin == 0)
                {
                    if (str.Found("lva:</span>")) //nincs mindig dátum kiírva
                    {
                        str.Find("lva:</span>"); str.Skip();
                        _date = str.Read("</p>"); str.Skip();

                        DateTime date = DateTime.Parse(_date);

                        //_date = date.ToShortDateString().Replace(" ", "");
                        _date = date.ToString("yyyy.MM.dd.");
                    }
                    else
                    {
                        //_date = "Now_"+DateTime.Now.ToShortDateString().Replace(" ", "");
                        _date = "Now_" + DateTime.Now.ToString("yyyy.MM.dd.");
                    }

                    writer = new StreamWriter(Path.Combine(ChartPath, _date + " (Latest).txt"));
                }

                string[] separator1 = { " - ", " – " };
                string[] separator2 = { "-", "–" };

                while (str.Find("<td class=\"chartContent\">"))
                {
                    var song = new CSong();

                    str.Find("alt=\""); str.Skip();
                    string artistTitle = str.Read("\""); str.Skip();
                    string[] arr = artistTitle.Split(separator1, StringSplitOptions.None);
                    try
                    {
                        song.Artist = arr[0].Trim();
                        song.Title = arr[1].Trim();
                    }
                    catch (Exception)
                    {
                        arr = artistTitle.Split(separator2, StringSplitOptions.None);
                        try
                        {
                            song.Artist = arr[0].Trim();
                            song.Title = arr[1].Trim();
                        }
                        catch (Exception)
                        { }
                    }

                    str.Find("<td class=\"current\">"); str.Skip();
                    song.Position = str.Read("</td>").Trim(); str.Skip();

                    str.Find("<td class=\"previous\">"); str.Skip();
                    string prevWeek = str.Read("</td>").Trim(); str.Skip();

                    int position = Convert.ToInt32(song.Position);

                    try
                    {
                        int prevWeekInt = Convert.ToInt32(prevWeek);

                        if (position < prevWeekInt)
                            song.Movement = "up";
                        else if (position > prevWeekInt)
                            song.Movement = "down";
                        else //=
                            song.Movement = "non";
                    }
                    catch (Exception)
                    {
                        song.Movement = "new";
                    }

                    /*str.Find("<span class=\"artist\">"); str.Skip();
                    song.Artist = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

                    str.Find("<span class=\"track\">"); str.Skip();
                    song.Title = str.Read("</span>").Replace("&amp;", "&"); str.Skip();*/

                    ProcessArtist(ref song.Artist);
                    ProcessTitle(ref song.Title);

                    writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);

                    _list.Add(song);
                }
                var r = new Random(begin);
                int milliSec = r.Next(100, 1000);
                Thread.Sleep(milliSec);
            }

            writer.Close();

            return _list;
        }
    }
}
