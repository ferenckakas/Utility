using Services.Discogs;
using Services.Discogs.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Chart
{
    public class DiscogsRelease : CChartBase
    {
        private readonly string _label;

        public DiscogsRelease(string label)
        {
            _label = label;
        }

        public override async Task<List<CSong>> GetChart(string name, string url)
        {
            Name = name;
            var list = new List<CSong>();

            FileName = $@"{_logDir}\{_label}\{Name}.txt";
            if (!File.Exists(FileName)) FileName = FileName.Replace(".txt", " [Fixed].txt");

            if (File.Exists(FileName))
            {
                list = GetChartFromFile(FileName);
                return list;
            }

            FileName = $@"{_logDir}\{_label}\{Name}.txt";


            var discogsClient = new DiscogsClient();

            ReleaseResponse release = await discogsClient.GetRelease(url);
            
            foreach (Track track in release.Tracklist.Where(t => t.Type != "heading"))
            {
                string artist = "";
                track.Artists.ForEach(a => artist = $"{artist} {a.Name}");

                list.Add(new CSong
                {
                    Artist = artist.Trim(),
                    Title = track.Title.Trim(),
                    TotalTime = CSong.ConvertToSeconds(track.Duration)
                });
            }        

            return list;
        }
    }
}
