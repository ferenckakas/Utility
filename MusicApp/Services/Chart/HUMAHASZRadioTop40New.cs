using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Services.Chart
{
    public class HUMAHASZRadioTop40New : CChart
    {
        public HUMAHASZRadioTop40New()
        {
            _countryCode = "HU"; _country = "Hungary";
            _name = "MAHASZ Radio Top 40";
            //sChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            _chartFrequency = new TimeSpan(10, 0, 0, 0); //14 days
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;

            var songs = GetChart("http://zene.slagerlistak.hu/radios-top-40-jatszasi-lista");

            var writer = new StreamWriter(Path.Combine(ChartPath, _date + " (Latest).txt"));

            foreach (CSong song in songs)
            {
                writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title);
            }

            writer.Close();

            return songs;
        }

        public List<CSong> GetChart(string url)
        {
            _list = new List<CSong>();

            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = webclient.DownloadString(url);

            var str = new Str(content);

            string[] separator = { " - " };

            str.Find("id=\"tol_ig\""); str.Skip();
            str.Find(">"); str.Skip();
            string dates = str.Read("</div>"); str.Skip();

            string[] arr = dates.Split(separator, StringSplitOptions.None);
            _date = arr[1];

            DateTime date = DateTime.Parse(_date);

            //_date = date.ToShortDateString().Replace(" ", "");
            _date = date.ToString("yyyy.MM.dd.");
            //var writer = new StreamWriter(Path.Combine(sChartPath, _date + " (Latest).txt"));

            while (str.Find("class=\"lista_sor\""))
            {
                var song = new CSong();

                str.Find("class=\"no_sor\""); str.Skip();
                str.Find(">"); str.Skip();
                song.Position = str.Read("</td>").Trim(); str.Skip();
                song.Position = song.Position.Replace(".", "");

                str.Find("class=\"mult_het_sor\""); str.Skip();
                string innerContent = str.Read("</td>");

                var str2 = new Str(innerContent);
                string prevWeek;
                if (str2.Found("<img"))
                {
                    str2.Find("<img"); str2.Skip();
                    str2.Find(">"); str2.Skip();
                    prevWeek = str2.ReadToEnd();
                }
                else
                {
                    str2.Find("<span"); str2.Skip();
                    str2.Find(">"); str2.Skip();
                    prevWeek = str2.Read("</span>");
                }

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
                    if (prevWeek.Contains("ÚJ"))
                        song.Movement = "new";
                    else // re
                        song.Movement = "re-entry";
                }

                str.Find("class=\"eloado\""); str.Skip();
                str.Find(">"); str.Skip();
                song.Artist = str.Read("</span>");

                str.Find("<br"); str.Skip();
                str.Find(">"); str.Skip();
                song.Title = str.Read("<br"); str.Skip();

                ProcessArtist(ref song.Artist);
                ProcessTitle(ref song.Title);

//                writer.WriteLine(song.sPosition + "\t" + song.sMovement + "\t" + song.sArtist + "\t" + song.sTitle);

                _list.Add(song);
            }

//            writer.Close();

            return _list;
        }
    }
}
