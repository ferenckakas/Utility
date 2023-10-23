using Common;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Chart
{
    public abstract class CChartBase
    {
        public string Name { get; set; }
        public string Year { get; set; }
        public string Week { get; set; }
        public string Date { get; set; }
        public string FileName { get; set; }

        protected string _logDir = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Log";

        public CChartBase()
        {
        }

        public abstract Task<List<CSong>> GetChart(string name, string url);

        protected List<CSong> GetChartFromFile(string fileName)
        {
            var list = new List<CSong>();
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                List<string> fields = line._SplitByTab();

                var song = new CSong();
                song.Artist = fields[0].RemoveMultipleSpaces().Trim();
                song.Title = fields[1].RemoveMultipleSpaces().Trim();
                if (fields.Count > 2)
                    song.TotalTime = CSong.ConvertToSeconds(fields[2].RemoveMultipleSpaces().Trim());

                list.Add(song);
            }

            return list;
        }
    }
}
