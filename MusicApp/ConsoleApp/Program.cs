using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Services;
using Common;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Artist artist = Wikipedia.GetArtist("Roxette");

            //TextFile.Create(artist);


            //UKSinglesChart chart = new UKSinglesChart(@"C:\Projects\MusicApp\TextDB\Charts\UK Singles Chart");
            //chart.GetCurrentChart();






            //UKSinglesChart chart = new UKSinglesChart();
            //List<CSong> list = chart.SaveIntoDB(@"C:\Projects\MusicApp\TextDB\Charts\UK Singles Chart\2017.01.20.txt");

            //TextFile.SaveIntoDB(chart.Released, list);





            //Token token = new Token
            //{
            //    AccessToken = Global.Session<string>("AccessToken"),
            //    ExpiresOn = Global.Session<DateTime>("ExpiresOn"),
            //    RefreshToken = Global.Session<string>("RefreshToken"),
            //    TokenType = Global.Session<string>("TokenType")
            //};

            //Spotify.SavePlaylistsIntoTXT(token, "custom", "", "Inbox").Wait();





   //         Youtube.Post("custom", "", "Show").Wait();






            Console.ReadKey();
        }
    }
}
