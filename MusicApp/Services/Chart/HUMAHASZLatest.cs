using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Chart
{
    public class HUMAHASZLatest : CChartBase
    {
        public HUMAHASZLatest()
        {
        }

        public override async Task<List<CSong>> GetChart(string name, string url)
        {
            Name = name;
            var list = new List<CSong>();

            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = await Task.Run(() => webclient.DownloadString(url));

            var str = new Str(content);

            string[] separator = { " - " };

            str.Find("id=\"ev_het\""); str.Skip();
            str.Find(">"); str.Skip();
            string yearWeek = str.Read("</div>"); str.Skip();
            var strYearWeek = new Str(yearWeek);
            Year = strYearWeek.Read("."); strYearWeek.Skip();
            Week = strYearWeek.Read(".");

            //Name = $"(Week {Year.Trim()}/{Week.Trim()})";

            FileName = $@"{_logDir}\MAHASZ\{Name} ({Year} Week {Week}).txt";
            if (!File.Exists(FileName)) FileName = FileName.Replace(".txt", " [Fixed].txt");
            
            if (File.Exists(FileName))
            {
                list = GetChartFromFile(FileName);
                return list;
            }

            FileName = $@"{_logDir}\MAHASZ\{Name} ({Year} Week {Week}).txt";

            str.Find("id=\"tol_ig\""); str.Skip();
            str.Find(">"); str.Skip();
            string dates = str.Read("</div>"); str.Skip();

            string[] arr = dates.Split(separator, StringSplitOptions.None);
            Date = arr[1];

            DateTime date = DateTime.Parse(Date);

            //Date = date.ToShortDateString().Replace(" ", "");
            Date = date.ToString("yyyy.MM.dd.");
            //            var writer = new StreamWriter(Path.Combine(sChartPath, Date + " (Latest).txt"));

            while (str.Find("class=\"no_sor\""))
            {
                var song = new CSong();

                str.Skip();
                str.Find(">"); str.Skip();
                song.Position = str.Read("</td>").Trim(); str.Skip();
                song.Position = song.Position.Replace(".", "");

                str.Find("class=\"mult_het_sor\""); str.Skip();
                string innerContent = str.Read("</td>");

                var str2 = new Str(innerContent);
                string prevWeek;
                if (str2.Found("<i"))
                {
                    str2.Find("<i"); str2.Skip();
                    str2.Find("</i>"); str2.Skip();
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

                list.Add(song);
            }

            return list;
        }
    }
}
