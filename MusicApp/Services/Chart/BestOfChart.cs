using System.Collections.Generic;
using System.IO;

namespace Services.Chart
{
    public class BestOfChart : CChart
    {
        public BestOfChart(string countryCode, string country, string chartName, string date)
        {
            _countryCode = countryCode;
            _country = country;
            _name = chartName;
            _date = date;

            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _countryCode + @"\" + _name;
            //_chartFrequency = new TimeSpan(30, 0, 0, 0); //7 days 21 hours
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
            var reader = new StreamReader(FileName); // System.Text.Encoding.Default?

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
                song.ImageUrl = "";
                song.Date = "";

                _list.Add(song);
                i++;
            }

            reader.Close();

            return _list;
        }

        public override bool ExistsNew()
        {
            return false;
        }
    }
}
