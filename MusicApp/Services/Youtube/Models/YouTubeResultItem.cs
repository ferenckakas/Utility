using System.Collections.Generic;

namespace Services.Mvc.Models
{
    public class YouTubeResultItem
    {
        public IList<Common.Models.SearchResult> Tracks;
        public Common.Models.SearchResult FoundTrack;

        public List<string> Artists;
        public string Artist;
        public string Title;
        public string TitleOriginal;
        public string Keywords;

        public string Rule;

        public List<YSearchAct> YSearchActs = new List<YSearchAct>();

        //var state = new State { Artists = artists, Artist = artists[0], Song = title, Keywords = keywords, Playlist = playlist, Tracks = tracks };
    }
}
