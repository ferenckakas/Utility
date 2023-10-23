using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Services;
using Services.Chart;

namespace ConsoleApp
{
    public static class TextFile
    {
        public static void Create(Artist artist)
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("@" + artist.Name);

            foreach (Album album in artist.Albums)
            {
                content.AppendLine("");
                string date = album.Released.HasValue ? album.Released.Value.ToShortDateString() : "";
                content.AppendLine("#" + album.Name + "\t" + date);

                foreach(AlbumSong albumSong in album.AlbumSongs.OrderBy(s => s.Number))
                {
                    content.AppendLine(albumSong.Number + ". " + albumSong.Song.Title);
                }
            }

            File.WriteAllText(@"C:\Projects\MusicApp\TextDB\Artists\"+ artist.Name + ".txt", content.ToString(), Encoding.UTF8);
        }

        public static void SaveIntoDB(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            Artist artist = null;
            Album album = null;
            Song song = null;

            foreach(string line in lines)
            {
                if (line.Trim().StartsWith("@"))
                {
                    string name = line.Trim().Substring(1);
                    artist = Artist.GetArtist(name);
                }
                else if (line.Trim().StartsWith("#"))
                {
                    string name = line.Trim().Substring(1);
                    album = Album.GetAlbum(artist, name);
                }
                else if (line.Trim().Length > 0)
                {
                    string title = line.Trim().Substring(1);
                    song = Song.GetSong(album, title);
                }

            }

        }

        public static void SaveIntoDB(DateTime Released, List<CSong> list)
        {
            Chart chart = new Chart();
            chart.Name = "UK Singles Chart";

            ChartReleas chartRelease = new ChartReleas();
            chartRelease.Released = Released;

            chart.ChartReleases.Add(chartRelease);

            foreach (CSong song in list)
            {
                ChartItem chartItem = new ChartItem();
                chartItem.ArtistName = song.sArtist;
                chartItem.Title = song.sTitle;

                chartRelease.ChartItems.Add(chartItem);
            }

            Main.Save(chart);
        }
    }
}