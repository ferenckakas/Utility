using Common;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Chart
{
    public class HUMAHASZAggregated : CChartBase
    {
        public HUMAHASZAggregated()
        {
        }

        public override async Task<List<CSong>> GetChart(string name, string url)
        {
            Name = name;
            var list = new List<CSong>();

            FileName = $@"{_logDir}\MAHASZ\Aggregated\{Name}.txt";
            if (!File.Exists(FileName)) FileName = FileName.Replace(".txt", " [Fixed].txt");

            if (File.Exists(FileName))
            {
                list = GetChartFromFile(FileName);
                return list;
            }

            FileName = $@"{_logDir}\MAHASZ\Aggregated\{Name}.txt";


            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string content = await Task.Run(() => webclient.DownloadString(url));

            var str = new Str(content);

            str.Find("<h3>"); str.Skip();
            string title = str.Read("</h3>"); str.Skip();

            while (str.Find("class=\"no_sor\""))
            {
                var song = new CSong();

                str.Find("class=\"eloado\""); str.Skip();
                str.Find(">"); str.Skip();
                song.Artist = str.Read("</span>");

                str.Find("<br"); str.Skip();
                str.Find(">"); str.Skip();
                song.Title = str.Read("<br"); str.Skip();
                song.Title = song.Title.Trim().TrimQuots();

                list.Add(song);
            }

            return list;
        }
    }
}
