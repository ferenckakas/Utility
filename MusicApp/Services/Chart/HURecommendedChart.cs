using System;
using System.Collections.Generic;
using System.IO;

namespace Services.Chart
{
    public class HURecommendedChart : CChart
    {
        private readonly string _chartFullName;

        public HURecommendedChart()
        {
            _countryCode = "HU"; _country = "Hungary";
            _name = "Recommended";
            //sChartPath = MvcApplication.CorePath + @"\Charts\" + _countryCode + @"\" + _name;
            //_chartFrequency = new TimeSpan(7, 21, 0, 0); //7 days 21 hours
            _chartFullName = ChartPath + @"\" + _name.ToLower() + ".txt";
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
                song.VideoID = "";
                song.ImageUrl = "";
                song.Date = "";

                _list.Add(song);
                i++;
            }

            reader.Close();

            return _list;
        }
    }
}
