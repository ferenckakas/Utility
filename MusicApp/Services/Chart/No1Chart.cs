using System.Collections.Generic;
using System.IO;

namespace Services.Chart
{
    public class No1Chart : CChart
    {
        public No1Chart(string country, string chartName, string date)
        {
            //_country = country;
            //_name = chartName;
            _date = date;
                       

            //ChartPath = MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            ////_chartFrequency = new TimeSpan(30, 0, 0, 0); //7 days 21 hours

            _countryCode = "UK"; _country = "United Kingdom";
            _name = "Singles Chart";
            ChartPath = ""; // MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
        }

        public override string PlaylistName
        {
            get
            {
                return _country + " - " + _name + " Number Ones " + _date;
            }
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
                return null;

            var reader = new StreamReader(Path.Combine(ChartPath, _date + ".txt")); // System.Text.Encoding.Default?

            if (reader == null) return null;

            _list = new List<CSong>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');

                var song = new CSong();

                song.Position = song.ReachedNo1 = fields[0];
                song.Movement = song.WeeksAtNo1 = fields[1];
                song.Artist = fields[2];
                song.Title = fields[3];

                if (fields.Length > 4)
                    song.VideoID = fields[4];
                else
                    song.VideoID = "";

                if (fields.Length > 5)
                    song.ImageUrl = fields[5];
                else
                    song.ImageUrl = "";

                song.Date = "";

                _list.Add(song);
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
