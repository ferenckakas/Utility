using System.Collections.Generic;

namespace Helper
{
    public class YoutubeChannel
    {
        private readonly List<string> _preferredTitles = new List<string>()
        {
            "Magneoton",
            "GoldRecord Hungary"
        };

        //public static List<string> preferredChannelIds = new List<string>();
        public static List<string> PreferredChannelUserNames = new List<string>()
        {
            "SpinninRec",        // 9,789 videos, 15,320,682 subscribers, dance
            "UltraRecords",      // 3,012 videos,  7,163,966 subscribers, electronic, dance, US
            "kontor",            // >1000 videos,  3,905,444 subscribers, electronic, dance,
            "AtlanticVideos",    //                5,192,443 subscribers
            "emimusic",          //                6,614,324 subscribers
            "armadamusic",       // 19,110 videos, 2,748,158 subscribers, Netherlands
            "warnermusicde",     //                  795,381 subscribers, DE
            "egoitaly",          //                  635,075 subscribers, Italy, dance        
                
            "datarecordsuk",     //                  377,492 subscribers 
            "Mostv",             // MinistryofSound  518,718 subscribers, dance,  little bit of trance, the very best house music, drum and bass/dubstep
            "MinistryOfSoundUK", //                   66,022 subscribers

            "SteveAATW",         //                  339,206 subscribers, dance, UK            
            "Magneoton",         //                   155,510 subscribers
            "lostikipictures"    //                    57,311 subscribers
        };

        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsPreferred {
            get
            {
                return _preferredTitles.Contains(Title);
            }
        }

        public bool IsVevo { get; set; }

        public YoutubeChannel(string id, string title)
        {
            Id = id;
            Title = title;

            if (string.IsNullOrWhiteSpace(Title))
                return;

            string titleWithoutVevo = Title;

            if (Title.ToLower().EndsWith("vevo"))
            {
                IsVevo = true;
                titleWithoutVevo.Substring(0, titleWithoutVevo.Length - 4);
            }

            titleWithoutVevo = titleWithoutVevo.Replace("Official", "");

            Text = titleWithoutVevo[0].ToString();

            for (var i = 1; i < titleWithoutVevo.Length; i++)
            {
                char c = titleWithoutVevo[i];
                if (char.IsWhiteSpace(c))
                    Text = $"{Text} ";
                else if (char.IsUpper(c))
                    Text = $"{Text} {c}";
                else
                    Text = $"{Text}{c}";
            }
        }
    }
}
