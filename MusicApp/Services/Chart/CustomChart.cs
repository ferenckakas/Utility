using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using Google.Apis.YouTube.v3.Data;

namespace Services.Chart
{
    public class CustomChart : CChart
    {
        public CustomChart(string country, string chartName, string date)
        {
            _country = country;
            _name = chartName;
            _date = date.EndsWith(".") ? date : date + ".";

            ChartPath = Common.Constant.TextDBPath + @"\Playlists"; // MvcApplication.CorePath + @"\Charts\" + _country + @"\" + _name;
            //_chartFrequency = new TimeSpan(30, 0, 0, 0); //7 days 21 hours
        }

        public override string PlaylistName
        {
            get
            {
                return _date;
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
            _list = new List<CSong>();

            string fileName = Path.Combine(ChartPath, _date + "txt");
            if (File.Exists(fileName))
            {
                var reader = new StreamReader(fileName); // System.Text.Encoding.Default?

                if (reader == null) return null;

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
                    song.VideoID = fields.Length > 2 ? fields[2] : "";
                    song.SpotifyID = fields.Length > 3 ? fields[3] : "";
                    song.ImageUrl = "";
                    song.Date = "";

                    _list.Add(song);
                    i++;
                }

                reader.Close();
            }
            return _list;
        }

        public override bool ExistsNew()
        {
            return false;
        }

        public override void Save(List<CSong> list)
        {
            //string[] lines = new string[list.Count];

            //foreach (CSong song in list)
            //{
            // //   song.Artist, song.Title, song.VideoID, song.SpotifyID
            //}

            List<string> l = list.Select(s => String.Format("{0}\t{1}\t{2}\t{3}", s.Artist, s.Title, s.VideoID, s.SpotifyID)).ToList();
            
            File.WriteAllLines(Path.Combine(ChartPath, _date + ".txt"), l.ToArray(), Encoding.UTF8);
        }
    }
}
