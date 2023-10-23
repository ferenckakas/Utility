using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Common;

namespace Helper
{
    public class YoutubeTrack
    {
        public string Ordinal { get; set; }
        public string VideoId { get; set; }
        public string ThumbnailUrl { get; set; }

        private Image _thumbnail;

        public Image Thumbnail
        {
            get
            {
                string fileName = $@"C:\Users\Ferenc\OneDrive\Documents\My App Data\ChartParser\Youtube\{VideoId}_default.jpg";
                return Utils.CreateImageFileIfNotExists(_thumbnail, fileName, ThumbnailUrl);
            }

            set
            {
                _thumbnail = value;
            }
        }

        public string Title { get; set; }
        public string Artist { get; set; }
        public string Name { get; set; }        
        public string Feat { get; set; }
        public string SongVersion { get; set; }

        // Discogs only
        public uint? Year { get; set; }

        public string VideoVersion { get; set; }

        public string Official { get; set; }

        public bool IsMix
        {
            get
            {
                return SongVersion != null &&
                       !SongVersion.ToLower()._ToWords().Contains("radio") &&
                       !SongVersion.ToLower().Contains("explicit version") &&
                       !SongVersion.ToLower().Contains("main version") &&
                       !SongVersion.ToLower().Contains("uk version") &&
                       !SongVersion.ToLower().Contains("us version") &&
                       !SongVersion.ToLower()._ToWords().Contains("video");
            }
        }

        public bool IsVideo
        {
            get
            {
                return VideoVersion == null;
            }
        }

        public bool IsLyric
        {
            get
            {
                if (VideoVersion == null)
                    return false;

                var lyricList = new List<string> { "lyric", "lyrics", "lyrich" };

                List<string> words = VideoVersion._ToWords();
                foreach (string f in lyricList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsAudio
        {
            get
            {
                if (VideoVersion == null)
                    return false;

                var audioList = new List<string> { "audio" };

                List<string> words = VideoVersion._ToWords();
                foreach (string f in audioList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsOfficial
        {
            get
            {
                return Official != null;
            }
        }

        public bool IsRemaster
        {
            get
            {
                if (SongVersion != null)
                {
                    List<string> words = SongVersion.ToLower()._ToWords();
                    return words.Contains("remaster") || words.Contains("remastered") || words.Contains("remastering");
                }
                else
                    return false;
            }
        }

        public int DurationInMilliSec { get; set; }
        public string ChannelId { get; set; }
        public string ChannelTitle { get; set; }
        public YoutubeChannel Channel { get; set; }

        // Discogs only
        public List<string> Names { get; set; }



        private readonly List<string> _songVersionList = new List<string>()
            { "mix", "remix", "dancemix", "extended", "edit", "version", "cut", "mashup", "reboot", "remaster", "remastered", "cover", "instrumental", "karaoke", "live", "acoustic", "beyond the video", "élő" };

        private readonly List<string> _featList = new List<string>()
            { "featuring", "feat.", "feat", "ft.", "ft", "with", "km.", "km" };

        //Dj Harmath feat.Reni - Álmaimban ( Shabba Summer 2k17 Club Mix )

        private readonly List<string> _videoVersionList = new List<string>() { "audio", "lyric", "lyrics", "lyrich", "teaser", "beyond the video" };

        private readonly List<string> _officialList = new List<string>() { "official", "officiel", "Oficial", "hivatalos" };

        private readonly List<string> _superfuiltyList = new List<string>() { "hd", "hq", "music video", "video", "videoclip", "ultra music", "out now", "free download", "soundtrack", "eurovision", "from" };


        private string _artistTerm;
        private string _nameTerm;
        private string _featTerm;
        private string _versionTerm;

        public void Decompose()
        {
            Decompose(_artistTerm, _nameTerm, _featTerm, _versionTerm);
        }

        public void DecomposeSpotify(string artistTerm, string nameTerm, string featTerm, string versionTerm)
        {
            _artistTerm = artistTerm;
            _nameTerm = nameTerm;
            _featTerm = featTerm;
            _versionTerm = versionTerm;

            string name = Title;

            DecomposeName(name);

            if (SongVersion == null) return;

            SongVersion = SongVersion.Trim();
            if (SongVersion.StartsWith("-"))
                SongVersion = SongVersion.Substring(1);
            SongVersion = SongVersion.Trim();
        }

        public void DecomposeDiscogs(string artistTerm, string nameTerm, string featTerm, string versionTerm)
        {
            _artistTerm = artistTerm;
            _nameTerm = nameTerm;
            _featTerm = featTerm;
            _versionTerm = versionTerm;

            string[] parts = Title.Split(new string[] { " - " }, StringSplitOptions.None);

            string artist = null;
            string name = null;

            switch (parts.Length)
            {
                case 1:
                    artist = parts[0].Trim();
                    name = parts[0].Trim();
                    Artist = artist.Trim();
                    Name = name.Trim();
                    Names = new List<string>() { Name };
                    return;
                case 2:
                    artist = parts[0].Trim();
                    name = parts[1].Trim();
                    break;
                default:
                    break;
            }

            if (parts.Length > 2)
            {
                artist = parts[0].Trim();
                name = string.Join(" - ", parts.Skip(1));
            }

            string[] names = name.Trim().Split(new string[] { " / " }, StringSplitOptions.None);

            if (names.Length > 1)
            {
                Name = name.Trim();
                Names = names.ToList();
            }
            else
            {
                DecomposeName(name);
                Names = new List<string>() { Name };
            }

            DecomposeArtist(artist);
            //DecomposeFrees(frees);
        }

        public void Decompose(string artistTerm, string nameTerm, string featTerm, string versionTerm)
        {
            _artistTerm = artistTerm;
            _nameTerm = nameTerm;
            _featTerm = featTerm;
            _versionTerm = versionTerm;

            string[] parts = Title.Split(new string[] { " - ", " – ", " — ", "- ", "– ", "— ",
                                                        " | ",
                                                        " • ", "• ",
                                                        " ::: ", " :: ", " : ", "::: ", ":: ", ": " }, StringSplitOptions.None);

            string artist = null;
            string name = null;
            var frees = new List<string>();

            if (parts.Length == 1)
            {
                parts = Title.Split(new string[] { "-", "–", "—", "|", "•", ":::", "::", ":" }, StringSplitOptions.None);
            }

            switch (parts.Length)
            {
                case 1:
                    string part = parts[0].Trim();

                    if (part.ToLower().Contains(artistTerm.ToLower()))
                        Artist = artistTerm;
                    else
                        Artist = part;

                    if (part.ToLower().Contains(nameTerm.ToLower()))
                        Name = nameTerm;
                    else
                        Name = part;


                    List<string> list = part.CaptureBrackets();

                    ParseFeat(list);

                    int idx = ParseSongVersion(list);
                    idx = ParseVideoVersion(list);
                    idx = ParseOfficial(list);

                    if (Feat == null && !string.IsNullOrWhiteSpace(featTerm))
                    {
                        if (part.ToLower().Contains(featTerm.ToLower()))
                            Feat = featTerm;
                    }

                    if (SongVersion == null && !string.IsNullOrWhiteSpace(versionTerm))
                    {
                        if (part.ToLower().Contains(versionTerm.ToLower()))
                            SongVersion = versionTerm;
                    }


                    string part2 = part.RemovePars().RemoveBrackets();

                    var listNotPar = new List<string> { part2 };

                    if (SongVersion == null)
                        ParseSongVersion(listNotPar);

                    if (VideoVersion == null)
                        ParseVideoVersion(listNotPar);

                    if (Official == null)
                        ParseOfficial(listNotPar);

                    return;
                case 2:
                    string left = parts[0].Trim();
                    string right = parts[1].Trim();

                    if (left._WordMatchPartial(artistTerm))
                    {
                        artist = left;
                        //if (right._WordMatchPartial(nameTerm)) 
                        name = right;
                    }
                    else if (right._WordMatchPartial(artistTerm))
                    {
                        artist = right;
                        if (left._WordMatchPartial(nameTerm)) name = left;
                    }
                    break;
                default:
                    break;
            }

            if (parts.Length > 2)
            {
                parts = parts.Select(p => p.Trim()).ToArray();
                foreach (string part in parts)
                {
                    if (part._WordMatchPartial(artistTerm))
                        artist = part;

                    if (part._WordMatchPartial(nameTerm))
                        name = part;
                }

                foreach (string part in parts)
                {
                    if (part != artist && part != name)
                        frees.Add(part);
                }
            }

            //string[] sepArtist = { " featuring ", " feat. ", " feat ", " ft. ", " ft ", " vs. ", " vs ", ", ", ",", " , ", " ," };
            //string[] sepArtist2 = { " & ", " and ", " + ", " x " };

            DecomposeName(name);
            DecomposeArtist(artist);
            DecomposeFrees(frees);
        }

        private void DecomposeName(string name)
        {
            if (name != null)
            {
                List<string> list = name.CaptureBrackets();
                //List<string> list2 = name.CapturePars();
                var leftover = new List<string>();

                if (list.Any())
                {
                    var removeIdxs = new List<int>();

                    ParseFeat(list);

                    int idx = ParseSongVersion(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    idx = ParseVideoVersion(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    idx = ParseOfficial(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    for (var i = 0; i < list.Count; i++)
                        if (!removeIdxs.Contains(i)) leftover.Add(list[i]);

                    RemoveSuperfluity(leftover);
                }

                name = name.RemovePars().RemoveBrackets();

                ParseFeatAndName(name, _nameTerm, _featTerm);

                //Name.RemoveSuperfluity("hd", "hq", ".flv", ".wmv", ".vob");

                //leftover = leftover.Select(e => $"({e})").ToList();
                //Name = $"{Name.Trim()} {string.Join(" ", leftover)}";

                Name = Name.Trim().TrimQuots();
            }
            else
                Name = "";
        }

        private void DecomposeArtist(string artist)
        {
            if (artist != null)
            {
                List<string> list = artist.CaptureBrackets();
                var leftover = new List<string>();

                if (list.Any())
                {
                    var removeIdxs = new List<int>();

                    ParseFeat(list);

                    int idx = ParseSongVersion(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    idx = ParseVideoVersion(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    idx = ParseOfficial(list);
                    if (idx > -1) removeIdxs.Add(idx);

                    for (var i = 0; i < list.Count; i++)
                        if (!removeIdxs.Contains(i)) leftover.Add(list[i]);

                    RemoveSuperfluity(leftover);
                }

                artist = artist.RemovePars().RemoveBrackets();

                ParseFeatAndArtist(artist, _artistTerm, _featTerm);

                //Name.RemoveSuperfluity("hd", "hq", ".flv", ".wmv", ".vob");

                //leftover = leftover.Select(e => $"({e})").ToList();
                //Artist = $"{Artist.Trim()} {string.Join(" ", leftover)}";

                Artist = Artist.RemoveAndFeat().Trim();
            }
            else
                Artist = "";
        }

        private void DecomposeFrees(List<string> frees)
        {
            //if (free != null)
            //{
            foreach (string free in frees)
            {
                List<string> list = free.CaptureBrackets();

                ParseFeat(list);

                int idx = ParseSongVersion(list);
                idx = ParseVideoVersion(list);
                idx = ParseOfficial(list);

                string free2 = free.RemovePars().RemoveBrackets();

                var listNotPar = new List<string> { free2 };

                idx = ParseSongVersion(listNotPar);
                idx = ParseVideoVersion(listNotPar);
                idx = ParseOfficial(listNotPar);

            }
        }


        // name ... feat. xxx zzz mix audio official

        // input
        // dancing
        // without (Queen)

        private void ParseFeatAndName(string text, string nameTerm, string featTerm)
        {
            if (text._StartsWith(nameTerm))
            {
                Name = text.Substring(0, nameTerm.Length);
                text = text.Substring(nameTerm.Length);
                if (string.IsNullOrWhiteSpace(text)) return;
            }

            List<string> words = text._ToWords();
            foreach (string f in _featList)
            {
                List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                int index = wordslowerCase.IndexOf(f);
                if (index > -1)
                {
                    text = string.Join(" ", words.GetRange(index + 1, words.Count - index - 1));
                    if (Feat == null)
                    {
                        if (featTerm != null && text._StartsWith(featTerm))
                        {
                            Feat = text.Substring(0, featTerm.Length);
                            text = text.Substring(featTerm.Length);
                        }
                        else
                        {
                            Feat = text;
                        }
                    }
                    if (Name == null) Name = string.Join(" ", words.GetRange(0, index));
                    break;
                }
            }

            if (Name == null) Name = text;

            ParseRemainingPart(text);
        }

        private void ParseFeatAndArtist(string text, string artistTerm, string featTerm)
        {
            if (text._StartsWith(artistTerm))
            {
                Artist = text.Substring(0, artistTerm.Length);
                text = text.Substring(artistTerm.Length);
                if (string.IsNullOrWhiteSpace(text)) return;
            }

            List<string> words = text._ToWords();
            foreach (string f in _featList)
            {
                List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                int index = wordslowerCase.IndexOf(f);
                if (index > -1)
                {
                    text = string.Join(" ", words.GetRange(index + 1, words.Count - index - 1));
                    if (Feat == null)
                    {
                        if (featTerm != null && text._StartsWith(featTerm))
                        {
                            Feat = text.Substring(0, featTerm.Length);
                            text = text.Substring(featTerm.Length);
                        }
                        else
                        {
                            Feat = text;
                        }
                    }
                    if (Artist == null) Artist = string.Join(" ", words.GetRange(0, index));
                    break;
                }
            }

            if (Artist == null) Artist = text;

            ParseRemainingPart(text);
        }

        private void ParseRemainingPart(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            if (SongVersion == null)
            {
                List<string> words = text._ToWords();
                foreach (string f in _songVersionList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        SongVersion = string.Join(" ", words);
                        //Name = string.Join(" ", words.GetRange(0, index));
                        return;
                    }
                }
            }

            if (VideoVersion == null)
            {
                List<string> words = text._ToWords();
                foreach (string f in _videoVersionList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        VideoVersion = string.Join(" ", words);
                        //Name = string.Join(" ", words.GetRange(0, index));
                        //return;
                    }
                }
            }

            if (Official == null)
            {
                List<string> words = text._ToWords();
                foreach (string f in _officialList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        Official = string.Join(" ", words);
                        //Name = string.Join(" ", words.GetRange(0, index));
                        //return;
                    }
                }
            }
        }




        private void ParseFeat(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _featList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        list.RemoveAt(i);
                        Feat = string.Join(" ", words.GetRange(index + 1, words.Count - index - 1));
                        return;
                    }
                }
            }
        }

        private int ParseSongVersion(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _songVersionList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        SongVersion = string.Join(" ", words);
                        return i;
                    }
                }
            }

            return -1;
        }

        private int ParseVideoVersion(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _videoVersionList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        VideoVersion = string.Join(" ", words);
                        return i;
                    }
                }
            }

            return -1;
        }

        private int ParseOfficial(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _officialList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        //list.RemoveAt(i);
                        Official = string.Join(" ", words);
                        return i;
                    }
                }
            }

            return -1;
        }


        private void RemoveSuperfluity(List<string> list)
        {
            var removeIdxs = new List<int>();

            for (var i = 0; i < list.Count; i++)
            {
                var words = new List<string> { list[i] };
                words.AddRange(list[i]._ToWords());
                foreach (string f in _superfuiltyList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        removeIdxs.Add(i);
                    }
                }
            }

            var leftover = new List<string>();
            for (var i = 0; i < list.Count; i++)
                if (!removeIdxs.Contains(i)) leftover.Add(list[i]);

            list = leftover;
        }
    }
}
