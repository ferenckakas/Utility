using System.Collections.Generic;

namespace Services.Mvc.Models
{
    public class YSearchAct
    {
        public string Title;
        public string Artist;
        public IList<Common.Models.SearchResult> Tracks;
    }
}
