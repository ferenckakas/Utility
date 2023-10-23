using Common;
using Entities;
using System;
using System.Net;
using System.Text;

namespace Services
{
    public static partial class Wikipedia
    {
        public static Artist GetArtist(string name)
        {
            var artist = new Artist();

            artist.Name = name;

            string url = GoogleManager.FindURL(name + " discography", "en.wikipedia.org", "en-US");
            if (url != "")
            {
                artist.WikiURL = url;

                //string urlExport = "https://en.wikipedia.org/wiki/Special:Export/" + url.Substring(url.LastIndexOf('/') + 1);

                var webclient = new WebClient();
                webclient.Encoding = Encoding.UTF8;

                string content = webclient.DownloadString(url);

                var str = new Str(content);

                string albumHeader = "";
                if (artist.WikiURL.Contains("discography"))
                    albumHeader = "Studio_albums";
                else
                    albumHeader = "Albums";

                str.Find("id=\"" + albumHeader + "\""); str.Skip();
                str.Find("<table ");
                Str table = str.Inner("</table>");

                table.Find("<tr"); table.Skip(); table.Find(">"); table.Skip();
                table.Find("<th"); table.Skip(); table.Find(">"); table.Skip();
                string header = table.Read("</th>").Trim();
                if (header.StartsWith("T")) //Title
                    GetStudioAlbumsFromTableTitle(ref artist, table);
                else if (header.StartsWith("Y")) //Year
                    GetStudioAlbumsFromTableYear(ref artist, table);

                str.Find("id=\"Singles\""); str.Skip();
                str.Find("<table ");
                table = str.Inner("</table>");

                table.Find("<tr"); table.Skip(); table.Find(">"); table.Skip();
                table.Find("<th"); table.Skip(); table.Find(">"); table.Skip();
                header = table.Read("</th>").Trim();
                if (header.StartsWith("T")) //Title
                    GetSinglesFromTableTitle(ref artist, table);
                else if (header.StartsWith("Y")) //Year
                    GetSinglesFromTableYear(ref artist, table);
            }

            return artist;
        }

        private static void GetStudioAlbumsFromTableTitle(ref Artist artist, Str table)
        {
            while (table.Find("<tr>"))
            {
                var studioAlbum = new Album();

                table.Skip();
                if (table.Find("<th scope=\"row\">"))
                {
                    table.Skip();
                    table.Find("<a"); table.Skip();
                    table.Find("href=\""); table.Skip();
                    studioAlbum.WikiURL = GetDomain(artist.WikiURL) + table.Read("\"").Trim();

                    table.Find(">"); table.Skip();
                    studioAlbum.Name = table.Read("</a>").Trim();

                    table.Find("Released:"); table.Skip();
                    studioAlbum.Released = DateTime.Parse(table.Read("<").Trim());

                    UpdateStudioAlbum(studioAlbum);

                    artist.Albums.Add(studioAlbum);
                }
            }
        }

        private static void GetSinglesFromTableTitle(ref Artist artist, Str table)
        {
            while (table.Find("<tr>"))
            {
                var single = new Album();

                table.Skip();
                if (table.Find("<th scope=\"row\">"))
                {
                    table.Skip();
                    table.Find("<a"); table.Skip();
                    table.Find("href=\""); table.Skip();
                    single.WikiURL = GetDomain(artist.WikiURL) + table.Read("\"").Trim();

                    table.Find(">"); table.Skip();
                    single.Name = table.Read("</a>").Trim();

                    //var song = new Song();
                    //song.Title = single.Name;
                    //AlbumSong albumSong = new AlbumSong();
                    //albumSong.Album = single;
                    //albumSong.Song = song;
                    //albumSong.Number = 1;
                    //single.AlbumSongs.Add(albumSong);

                    artist.Albums.Add(single);
                }
            }
        }

        private static void GetStudioAlbumsFromTableYear(ref Artist artist, Str table)
        {
            while (table.Find("<tr"))
            {
                Str tr = table.Inner("</tr>");

                if (tr.Find("<td"))
                {
                    tr.Skip(); tr.Find(">"); tr.Skip();
                    //studioAlbum.Released = DateTime.Parse(table.Read("<").Trim());

                    if (tr.Find("<td"))
                    {
                        Str td = tr.Inner("</td>");
                        var studioAlbum = new Album();

                        Str beforeul = td.Inner("<ul>");

                        if (beforeul.Find("<a"))
                        {
                            beforeul.Skip();
                            beforeul.Find("href=\""); beforeul.Skip();
                            studioAlbum.WikiURL = GetDomain(artist.WikiURL) + beforeul.Read("\"").Trim();

                            beforeul.Find(">"); beforeul.Skip();
                            studioAlbum.Name = beforeul.Read("</a>").Trim();
                        }
                        else
                        {
                            studioAlbum.Name = beforeul.ReadToEnd().Trim().RemoveHTML();
                        }

                        td.Find(">Released:"); td.Skip();
                        DateTime date;
                        DateTime.TryParse(td.Read("<").Trim(), out date);
                        studioAlbum.Released = date;

                        UpdateStudioAlbum(studioAlbum);

                        artist.Albums.Add(studioAlbum);
                    }
                }
            }
        }

        private static void GetSinglesFromTableYear(ref Artist artist, Str table)
        {
            while (table.Find("<tr"))
            {
                Str tr = table.Inner("</tr>");

                if (tr.Find("<td"))
                {
                    Str td = tr.Inner("</td>");

                    if (td.Read(1) != "\"")
                    {
                        //single.Released
                        tr.Find("<td");
                        td = tr.Inner("</td>");
                    }

                    if (td.Read(1) == "\"")
                    {
                        td.Skip();

                        var single = new Album();

                        if (td.Read(3) == "<a ")
                        {
                            if (td.Find("<a "))
                            {
                                td.Skip();
                                td.Find("href=\""); td.Skip();
                                single.WikiURL = GetDomain(artist.WikiURL) + td.Read("\"").Trim();

                                td.Find(">"); td.Skip();
                                single.Name = td.Read("</a>").Trim();
                            }
                        }
                        else
                        {
                            single.Name = td.Read("\"").Trim();
                        }

                        //var song = new Song();
                        //song.Title = single.Name;
                        //AlbumSong albumSong = new AlbumSong();
                        //albumSong.Album = single;
                        //albumSong.Song = song;
                        //albumSong.Number = 1;
                        //single.AlbumSongs.Add(albumSong);

                        artist.Albums.Add(single);
                    }
                }
            }
        }

        private static string GetDomain(string url)
        {
            int index = url.IndexOf(".org/");
            return url.Substring(0, index + 4);
        }
    }
}
