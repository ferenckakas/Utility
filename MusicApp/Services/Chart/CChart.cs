using Common;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Services.Chart
{
    public class CSong
    {
        public string Date;
        public string Position;
        public string Movement;
        public string Artist;
        public string Title;
        public string ImageUrl;
        public string VideoID;
        public string SpotifyID;

        public string ReachedNo1;
        public string WeeksAtNo1;

        public uint? TotalTime;

        public string DisplayTime
        {
            get
            {
                return TotalTime != null ? $"{(TotalTime / 60)}:{(TotalTime % 60).ToString().PadLeft(2, '0')}" : "";
            }
        }

        public static uint? ConvertToSeconds(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return null;

            string[] array = duration.Split(':');

            uint minutes = uint.Parse(array[0].Trim());
            uint seconds = uint.Parse(array[1].Trim());

            return minutes * 60 + seconds;
        }
    }

    public abstract class CChart
    {
        protected TimeSpan _chartFrequency;
        protected string _countryCode;
        protected string _country;
        protected string _name;
        public string ChartPath;
        protected List<CSong> _list;
        protected string _date;

        public string Country { get { return _country; } }
        public string Name { get { return _name; } }
        public string Date { get { return _date; } }

        public DateTime Released { get; set; }

        public string FileName { get { return Path.Combine(ChartPath, _date + "txt"); } }

        public List<CSong> List { get { return _list; } }

        //public static Dictionary<string, string> Countries = new Dictionary<string, string>();

        public virtual string PlaylistName {
            get
            {
                return _country + " - " + _name;
            }
        }


        static CChart()
        {
            //Countries.Add("US", "United States");
            //Countries.Add("UK", "United Kingdom");
            //Countries.Add("DE", "Germany");
            //Countries.Add("HU", "Hungary");
            //Countries.Add("JP", "Japan");           
        }

        public CChart()
        {
        }

        public abstract List<CSong> GetCurrentChart();

        public virtual void Save(List<CSong> list)
        {
        }

        public virtual bool ExistsNew()
        {
            //if (!Directory.Exists(sChartPath))
            //    Directory.CreateDirectory(sChartPath);

            string[] files = Directory.GetFiles(ChartPath);

            if (files.Length == 0)
                return true; // new chart

            string lastFile = files[files.Length - 1];

            DateTime lastDate;

            try
            {
                lastDate = DateTime.Parse(Path.GetFileNameWithoutExtension(lastFile));

                if (DateTime.Now.AddHours(9) > lastDate.Add(_chartFrequency)) //.AddDays(7).AddHours(21))
                    return true; // new chart
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }

        protected List<CSong> GetCachedChart()
        {
            //if (!Directory.Exists(sChartPath))
            //    Directory.CreateDirectory(sChartPath);

            string[] files = Directory.GetFiles(ChartPath);

            if (files.Length == 0)
                return null; // new chart

            string lastFile = files[files.Length - 1];

            DateTime lastDate;

            try
            {
                lastDate = DateTime.Parse(Path.GetFileNameWithoutExtension(lastFile).Replace("(Latest)", ""));

                if (DateTime.Now.AddHours(9) > lastDate.Add(_chartFrequency)) //.AddDays(7).AddHours(21))      //AddHours(9) because of timezone between Pasadena and Budapest
                {
                    foreach (string file in files)
                    {
                        if (file.EndsWith(" (Latest).txt"))
                        {
                            string newFile = Path.Combine(Path.GetDirectoryName(file), "_archive");
                            if (!Directory.Exists(newFile)) Directory.CreateDirectory(newFile);
                            newFile = Path.Combine(newFile, Path.GetFileName(file));
                            File.Move(file, newFile);
                        }
                    }

                    return null; // new chart
                }                    
            }
            catch (Exception)
            {
                return null;
            }

            //sDate = LastDate.ToShortDateString().Replace(" ", "");
            _date = lastDate.ToString("yyyy.MM.dd.");

            var reader = new StreamReader(lastFile); // System.Text.Encoding.Default?

            if (reader == null) return null;

            _list = new List<CSong>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] fields = line.Split('\t');

                var song = new CSong();

                song.Position = fields[0];
                song.Movement = fields[1];
                song.Artist = fields[2];
                song.Title = fields[3];
                if (fields.Length > 4)
                    song.ImageUrl = fields[4]; // new

                //if (sFields.Length > 4)
                //    song.sVideoID = sFields[4];
                //else
                //    song.sVideoID = "";

                //if (sFields.Length > 5)
                //    song.sImageUrl = sFields[5];
                //else
                //    song.sImageUrl = "";

                song.Date = "";

                _list.Add(song);
            }

            reader.Close();

            return _list;
        }

        protected List<CSong> GetChartFromDB()
        {
            using (var entities = new MusicEntities())
            {
                Entities.Chart chart = entities.Charts.FirstOrDefault(c => c.Name == Name);
                if (chart == null) return null;

                ChartReleas chartReleas = chart.ChartReleases.OrderByDescending(cs => cs.Released).FirstOrDefault();
                //chartReleas = chart.ChartReleases.FirstOrDefault(r => r.Released == Released);
                if (chartReleas == null) return null;
                if (DateTime.Now.AddHours(9) > chartReleas.Released.Add(_chartFrequency)) //.AddDays(7).AddHours(21))      //AddHours(9) because of timezone between Pasadena and Budapest
                    return null;

                _list = new List<CSong>();

                foreach (ChartReleaseSong chartReleaseSong in chartReleas.ChartReleaseSongs.OrderBy(cs => cs.Number))
                {
                    Song song = chartReleaseSong.Song;
                    var csong = new CSong();
                    csong.Artist = song.ArtistName;
                    csong.Title = song.Title;
                    _list.Add(csong);
                }

                if (_list.Count == 0)
                    return null;
            }

            return _list;
        }

        public List<CSong> GetNo1Chart(string sDate)
        {
            ////sChartPath = sChartPath + @"\No1s";
            //return GetChart(sDate);
            return null;
        }

        protected bool SaveCachedChart()
        {
            var writer = new StreamWriter(Path.Combine(ChartPath, _date + "txt"));

            foreach (CSong song in _list)
            {
                writer.WriteLine(song.Position + "\t" + song.Movement + "\t" + song.Artist + "\t" + song.Title + "\t" + song.VideoID + "\t" + song.ImageUrl);
            }

            writer.Close();

            return true;
        }

        public bool Update(string sPos)
        {
            //sPos

            foreach (CSong song in _list)
            {
                if (song.Position == sPos)
                {
                    string url = "http://www.youtube.com/results?search_query=" + song.Artist.Replace(" ", "+").Replace("&", "%26") + "+" + song.Title.Replace(" ", "+").Replace("&", "%26") + "&aq=f";

                    var webclient = new WebClient();

                    //webclient.Headers.Add(HttpRequestHeader.Host, "www.youtube.com");
                    //webclient.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
                    webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/13.0.782.220 Safari/535.1");
                    webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
                    webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
                    webclient.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");

                    string content = webclient.DownloadString(url);

                    //System.IO.Compression.GZipStream

                    var str = new Str(content);

                    str.Find("search-results"); str.Skip();
                    str.Find("result-item"); str.Skip();
                    str.Find("<a"); str.Skip();
                    str.Find("href=\"/watch?v="); str.Skip();
                    song.VideoID = str.Read("\""); str.Skip();

                    str.Find("<img"); str.Skip();
                    str.Find("src=\""); str.Skip();
                    song.ImageUrl = str.Read("\""); str.Skip();

                    break;
                }
            }

            SaveCachedChart();

            return true;
        }
        protected void ProcessArtist(ref string artist)
        {
            //artist = artist.Replace("!nk", "ink"); //pink
        }

        protected void ProcessTitle(ref string title)
        {
            //title = title.Replace("**k", "uck"); //fuck
        }

        public static CChart Instantiate(string path)
        {
            //return new ITunesChart("hu", "Hungary", "All");

            //string[] params = sCountry.Split('-');
            string[] @params = path.Split('/');

            string countryName = @params[0];
            string chartName = @params[1];

            if (@params.Length == 3 && chartName == "ituneschart")
            {
                //string sCountryCode = params[0];
                string genre = @params[2];

                Country country;

                //using (HitsEntities entities = new HitsEntities())
                //{
                //    country = entities.Countries.FirstOrDefault(x => x.ID == sCountryCode.ToLower());
                //}
                //return new ITunesChart(sCountryCode, country.Name, sGenre);

                string countryNameLower = countryName.Replace('-', ' ').ToLower();

                using (var entities = new MusicEntities())
                {
                    country = entities.Countries.FirstOrDefault(x => x.Name.ToLower() == countryNameLower);
                }
                return new ITunesChart(country.ID, country.Name, genre);
            }
            else if (countryName == "mychart")
            {
                return new MyChart(@params[1]);
            }
            else if (countryName == "united-states")
            {
                return new USChart();
            }
            else if (countryName == "united-kingdom" && chartName == "singleschart")
            {
                return new UKSinglesChart();
            }
            else if (countryName == "united-kingdom" && chartName == "dancesingles")
            {
                return new UKDanceSingles();
            }
            else if (countryName == "united-kingdom" && chartName == "rocksingles")
            {
                return new UKRockSingles();
            }
            else if (countryName == "hungary" && chartName == "vivachart")
            {
                return new HUVIVAChart();
            }
            else if (countryName == "hungary" && chartName == "vivaclubchart")
            {
                return new HUVIVAClubChart();
            }
            else if (countryName == "hungary" && chartName == "mahaszradiotop40")
            {
                return new HUMAHASZRadioTop40New();
            }
            else if (countryName == "hungary" && chartName == "mahaszdancetop40")
            {
                return new HUMAHASZDanceTop40();
            }
            else if (countryName == "hungary" && chartName == "recommended")
            {
                //return new BestOfChart("HU", "Hungary", "Recommended", "recommended.");
                return new HURecommendedChart();
            }
            else if (countryName == "hungary" && chartName == "2011")
            {
                return new BestOfChart("HU", "Hungary", "BestOf", "2011.");
            }
            else if (countryName == "hungary" && chartName == "2010")
            {
                return new BestOfChart("HU", "Hungary", "BestOf", "2010.");
            }
            else if (countryName == "hungary" && chartName == "2009")
            {
                return new BestOfChart("HU", "Hungary", "BestOf", "2009.");
            }
            else if (countryName == "international" && chartName == "chart2011")
            {
                return new BestOfChart("International", "International", "Chart 2011", "2011.");
            }
            else if (countryName == "international" && chartName == "chart2010")
            {
                return new BestOfChart("International", "International", "Chart 2010", "2010.");
            }
            else if (countryName == "international" && chartName == "chart2009")
            {
                return new BestOfChart("International", "International", "Chart 2009", "2009.");
            }
            //else if (countryName == "custom" && chartName == "private")
            //{
            //    return new CustomChart("Custom", "Private", sTime);
            //}
            else
            {
                return null;
            }
        }

        public void SaveIntoDB(DateTime released)
        {
            using (var entities = new MusicEntities())
            {
                Entities.Chart chart = Entities.Chart.GetChart(Name, entities); //"UK Singles Chart"

                ChartReleas chartRelease = ChartReleas.GetChartRelease(released, chart, entities);

                if (chartRelease.ID == 0)
                {
                    short i = 1;
                    foreach (CSong csong in _list)
                    {
                        //ChartItem chartItem = new ChartItem();
                        //chartItem.ArtistName = song.sArtist;
                        //chartItem.Title = song.sTitle;

                        //chartRelease.ChartItems.Add(chartItem);

                        Song song = entities.GetSong(csong.Artist, csong.Title);

                        ChartReleaseSong chartReleaseSong = new ChartReleaseSong();
                        chartReleaseSong.ChartReleas = chartRelease;
                        chartReleaseSong.Song = song;
                        chartReleaseSong.Number = i;

                        chartRelease.ChartReleaseSongs.Add(chartReleaseSong);

                        i++;
                    }

                    entities.SaveChanges();
                    //Main.Save(chart);
                }
            }
        }
    }
}
