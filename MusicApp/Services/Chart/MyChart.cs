using System;
using System.Collections.Generic;
using System.IO;

namespace Services.Chart
{
    public class MyChart : CChart
    {
        private readonly string _chartFullName;

        private Dictionary<string, string> _myChartDict;
        private Dictionary<string, string> _myChartVideoIDDict;

        public MyChart(string chartName)
        {
            //_countryCode = "MyCharts";
            //_country = "MyCharts";

            _countryCode = "HU";
            //_country = "MyCharts";

            _name = chartName;

            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _countryCode + @"\" + _name;
            _chartFullName = ChartPath + @"\" + chartName.ToLower() + ".txt";
        }

        public override List<CSong> GetCurrentChart()
        {
            if (_list != null) return _list;

            _list = GetCachedChart();

            if (_list != null) return _list;

            return _list;
        }

        protected new List<CSong> GetCachedChart()
        {
            if (!Directory.Exists(ChartPath))
                Directory.CreateDirectory(ChartPath);

            StreamReader reader;

            try
            {
                reader = new StreamReader(_chartFullName); // System.Text.Encoding.Default?
            }
            catch (Exception)
            {
                return null;
            }

            if (reader == null) return null;

            _list = new List<CSong>();

            int i = 1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');

                var song = new CSong();

                song.Position = i.ToString();
                song.Movement = "";
                song.Artist = fields[0];
                song.Title = fields[1];
                song.VideoID = fields[2];
                song.ImageUrl = "";
                song.Date = "";

                _list.Add(song);
                i++;
            }

            reader.Close();

            return _list;
        }

        public Dictionary<string, string> GetChartDict()
        {
            if (_myChartDict != null) return _myChartDict;

            if (!Directory.Exists(ChartPath))
                Directory.CreateDirectory(ChartPath);

            StreamReader reader;

            try
            {
                reader = new StreamReader(_chartFullName); // System.Text.Encoding.Default?
            }
            catch (Exception)
            {
                return null;
            }

            if (reader == null) return null;

            _myChartDict = new Dictionary<string, string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');

                string key = fields[0] + "_" + fields[1];
                if (_myChartDict.ContainsKey(key))
                    _myChartDict.Add(key, "");
            }

            reader.Close();

            return _myChartDict;
        }

        public Dictionary<string, string> GetChartVideoIDDict()
        {
            if (_myChartVideoIDDict != null) return _myChartVideoIDDict;

            if (!Directory.Exists(ChartPath))
                Directory.CreateDirectory(ChartPath);

            StreamReader reader;

            try
            {
                reader = new StreamReader(_chartFullName); // System.Text.Encoding.Default?
            }
            catch (Exception)
            {
                return null;
            }

            if (reader == null) return null;

            _myChartVideoIDDict = new Dictionary<string, string>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');
                if (!_myChartVideoIDDict.ContainsKey(fields[2]))
                    _myChartVideoIDDict.Add(fields[2], "");
            }

            reader.Close();

            return _myChartVideoIDDict;
        }
    }
}
