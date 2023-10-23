using Common;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Helper
{
    public class CatalogTrack
    {
        public string CatalogId { get; set; }
        public string Ordinal { get; set; }
        public string ArtworkUrl { get; set; }
        private Image _artwork { get; set; }

        public Image Artwork
        {
            get
            {
                string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\AppleMusic\Catalog\{CatalogId}.jpeg";
                return Utils.CreateImageFileIfNotExists(_artwork, fileName, ArtworkUrl?.Replace("{w}x{h}", "50x50")); //600x600
            }

            set
            {
                _artwork = value;
            }
        }

        public string RawName { get; set; }
        public string RawArtist { get; set; }

        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public uint? TotalTime { get; set; }
        public string DisplayTime
        {
            get
            {
                return TotalTime != null ? $"{(TotalTime / 60)}:{(TotalTime % 60).ToString().PadLeft(2, '0')}" : "";
            }
        }

        public string ReleaseDate { get; set; }
        public string Genre { get; set; }
        //public uint? Year { get; set; }

        public bool IsMix { get; set; }

        //public bool IsMix {
        //    get
        //    {
        //        return isMix.Value;
        //    }
        //}

        public void Decompose(string title)
        {
            RawName = Name;
            RawArtist = Artist;

            if (!Name._Match(title))
            {
                string nameWithoutMixPart = Name;
                if (nameWithoutMixPart.IndexOf("[") > -1) nameWithoutMixPart = nameWithoutMixPart.Substring(0, nameWithoutMixPart.IndexOf("[")).Trim();

                bool mix1 = false, mix2 = false;
                string newName = Name;
                if (!nameWithoutMixPart._Match(title))
                {
                    List<string> pList = Name.ToLower().Capture(@"\(([^()]*)\)");
                    if (pList.Any(e => e._ToWords().Contains("mix") || e._ToWords().Contains("mixed") || e._ToWords().Contains("remix") ||
                                       e._ToWords().Contains("edit") || e._ToWords().Contains("version") || e._ToWords().Contains("extended") ||
                                       e._ToWords().Contains("instrumental") || e._ToWords().Contains("karaoke") || e._ToWords().Contains("cover") ||
                                       e._ToWords().Contains("live") || e._ToWords().Contains("acoustic")))
                        if (pList.All(e => !e._Match("radio mix")) &&
                            pList.All(e => !e._Match("radio edit")) &&
                            pList.All(e => !e._Match("radio version")))
                            mix1 = true;
                        else
                            mix1 = false;
                    else
                    {
                        mix1 = false;
                        pList.ForEach(e => Artist += " " + e);
                    }

                    if (newName.IndexOf("(") > -1) newName = newName.Substring(0, newName.IndexOf("(")).Trim();
                }

                List<string> bList = Name.ToLower().Capture(@"\[([^\[\]]*)\]");
                if (bList.Any(e => e._ToWords().Contains("mix") || e._ToWords().Contains("mixed") || e._ToWords().Contains("remix") ||
                                   e._ToWords().Contains("edit") || e._ToWords().Contains("version") || e._ToWords().Contains("extended") ||
                                   e._ToWords().Contains("instrumental") || e._ToWords().Contains("karaoke") || e._ToWords().Contains("cover") ||
                                   e._ToWords().Contains("live") || e._ToWords().Contains("acoustic")))
                    if (bList.All(e => !e._Match("radio mix")) &&
                        bList.All(e => !e._Match("radio edit")) &&
                        bList.All(e => !e._Match("radio version")))
                        mix2 = true;
                    else
                        mix2 = false;
                else
                {
                    mix2 = false;
                    bList.ForEach(e => Artist += " " + e);
                }

                IsMix = mix1 || mix2;

                if (newName.IndexOf("[") > -1) newName = newName.Substring(0, newName.IndexOf("[")).Trim();

                Name = newName;
            }

            Artist = Artist.RemoveAndFeat().RemoveMultipleSpaces().Trim();
        }
    }
}
