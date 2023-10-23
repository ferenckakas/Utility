using Common;
using Entities;
using System.Net;
using System.Text;

namespace Services
{
    public static partial class Wikipedia
    {
        public static void UpdateStudioAlbum(Album album)
        {
            var webclient = new WebClient();
            webclient.Encoding = Encoding.UTF8;

            string url = album.WikiURL;

            string content = webclient.DownloadString(url);

            var str = new Str(content);

            str.Find(" class=\"infobox"); str.Skip();
            str.Find("<a"); str.Skip();
            str.Find("<img"); str.Skip();
            str.Find(" srcset=\""); str.Skip();
            album.ImageURL = str.Read("\"");

            int index = album.ImageURL.LastIndexOf(" ");
            if (index > -1) album.ImageURL = album.ImageURL.Substring(0, index);

            str.Find("id=\"Track_list"); str.Skip();
            str.Find("</h"); str.Skip();
            string hlevel = str.Read(">");

            var h = new Str(str.Read("<h" + hlevel));

            if (h.Find("<table class=\"tracklist\""))
                UpdateFromTable(h, album);
            else if (h.Find("<ol"))
                UpdateFromList(h, album);
        }

        private static void UpdateFromTable(Str h, Album album)
        {
            Str table = h.Inner("</table>");

            table.Find("<th");
            Str th = table.Inner("</th>");
            bool SideOne = th.ReadToEnd().Trim().Contains("Side ");

            while (table.Find("<tr"))
            {
                Str tr = table.Inner("</tr>");
                if (tr.Find("<td"))
                {
                    Str td = tr.Inner("</td>");
                    string number = td.ReadToEnd().Trim().Replace(".", "");
                    if (number.IsNumber())
                    {
                        tr.Skip();
                        tr.Find("<td");
                        td = tr.Inner("</td>");

                        if (td.Read(1) == "\"")
                        {
                            var song = new Song();
                            //song.Artist
                            //song.ArtistName = album.Artist.Name;

                            td.Skip();
                            if (td.Read(3) == "<a ")
                            {
                                if (td.Find("<a "))
                                {
                                    td.Skip();
                                    td.Find("href=\""); td.Skip();
                                    song.WikiURL = td.Read("\"").Trim();

                                    td.Find(">"); td.Skip();
                                    song.Title = td.Read("</a>").Trim();
                                }
                            }
                            else
                            {
                                song.Title = td.Read("\"").Trim();
                            }

                            var albumSong = new AlbumSong();
                            albumSong.Album = album;
                            albumSong.Song = song;
                            albumSong.Number = byte.Parse(number);

                            album.AlbumSongs.Add(albumSong);
                        }
                    }
                }
            }

            h.Skip();
            if (h.Find("<table class=\"tracklist\""))
            {
                table = h.Inner("</table>");

                table.Find("<th");
                th = table.Inner("</th>");
                bool sideTwo = th.ReadToEnd().Trim().Contains("Side ");

                if (SideOne && sideTwo)
                {
                    while (table.Find("<tr"))
                    {
                        Str tr = table.Inner("</tr>");
                        if (tr.Find("<td"))
                        {
                            Str td = tr.Inner("</td>");
                            string number = td.ReadToEnd().Trim().Replace(".", "");
                            if (number.IsNumber())
                            {
                                tr.Skip();
                                tr.Find("<td");
                                td = tr.Inner("</td>");

                                if (td.Read(1) == "\"")
                                {
                                    var song = new Song();
                                    //song.Artist
                                    //song.ArtistName = album.Artist.Name;

                                    td.Skip();
                                    if (td.Read(3) == "<a ")
                                    {
                                        if (td.Find("<a "))
                                        {
                                            td.Skip();
                                            td.Find("href=\""); td.Skip();
                                            song.WikiURL = td.Read("\"").Trim();

                                            td.Find(">"); td.Skip();
                                            song.Title = td.Read("</a>").Trim();
                                        }
                                    }
                                    else
                                    {
                                        song.Title = td.Read("\"").Trim();
                                    }

                                    var albumSong = new AlbumSong();
                                    albumSong.Album = album;
                                    albumSong.Song = song;
                                    albumSong.Number = byte.Parse(number); ;

                                    album.AlbumSongs.Add(albumSong);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void UpdateFromList(Str h, Album album)
        {
            Str ol = h.Inner("</ol>");

            byte number = 1;

            while (ol.Find("<li"))
            {
                Str li = ol.Inner("</li>");

                var song = new Song();
                //song.Artist
                //song.ArtistName = album.Artist.Name;

                if (li.Find("<a "))
                {
                    li.Skip();
                    li.Find("href=\""); li.Skip();
                    song.WikiURL = li.Read("\"").Trim();

                    li.Find(">"); li.Skip();
                    song.Title = li.Read("</a>").Trim();
                }
                else
                {
                    if (li.Find("\""))
                    {
                        li.Skip();
                        song.Title = li.Read("\"").Trim();
                    }
                    else
                        song.Title = li.ReadToEnd();
                }

                var albumSong = new AlbumSong();
                albumSong.Album = album;
                albumSong.Song = song;
                albumSong.Number = number++;

                album.AlbumSongs.Add(albumSong);
            }
        }
    }
}
