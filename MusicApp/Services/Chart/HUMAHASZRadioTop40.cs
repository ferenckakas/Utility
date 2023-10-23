using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Services.Chart
{
    public class HUMAHASZRadioTop40 : CChart
    {
        public HUMAHASZRadioTop40()
        {
            _countryCode = "HU"; _country = "Hungary";
            _name = "MAHASZ Radio Top 40";
            //sChartPath = MvcApplication.CorePath + @"\Charts\" + _countryCode + @"\" + _name;
            _chartFrequency = new TimeSpan(10, 0, 0, 0); //14 days
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;


            _list = new List<CSong>();

            string url = "http://www.mahasz.hu/?menu=slagerlistak&menu2=radios_top_40";

            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = webclient.DownloadString(url);

            var str = new Str(content);

            string[] separator = { " - " };

            str.Find("alt=\"Rádiós Top 40 játszási lista\""); str.Skip();
            str.Find("<td align=\"center\">"); str.Skip();
            string dates = str.Read("</td>"); str.Skip();

            string[] arr = dates.Split(separator, StringSplitOptions.None);
            _date = arr[1];

            DateTime date = DateTime.Parse(_date);

            //_date = date.ToShortDateString().Replace(" ", "");
            _date = date.ToString("yyyy.MM.dd.");
            var writer = new StreamWriter(Path.Combine(ChartPath, _date + "txt"));

            str.Find("<table "); str.Skip();
            str.Find("<tr bgcolor="); str.Skip();

            while (str.Find("<tr bgcolor="))
            {
                var song = new CSong();

                str.Find("<td><b>"); str.Skip();
                song.Position = str.Read("<b></td>"); str.Skip();

                str.Find("<td>"); str.Skip();

                str.Find("<td>"); str.Skip();
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
                    if (prevWeek.Contains("új"))
                        song.Movement = "new";
                    else // re
                        song.Movement = "re-entry";
                }

                str.Find("<td align=\"left\"><b>"); str.Skip();
                song.Artist = str.Read("</b>");
                if (song.Artist.Substring(0, 3) == "<a ")
                {
                    str.Find(">"); str.Skip();
                    song.Artist = str.Read("</a>"); str.Skip();
                }
                else
                    str.Skip();

                str.Find("<br>"); str.Skip();
                song.Title = str.Read("</td>"); str.Skip();
                
                /*str.Find("<span class=\"artist\">"); str.Skip();
                song.Artist = str.Read("</span>").Replace("&amp;", "&"); str.Skip();

                str.Find("<span class=\"track\">"); str.Skip();
                song.Title = str.Read("</span>").Replace("&amp;", "&"); str.Skip();*/

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
