//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Common.Models;

//namespace Common
//{
//    public static class Extension
//    {
//        public static void Filter(this List<SearchResult> catalogTracks, string rule, ref SearchResult track, Func<SearchResult, bool> Func, List<string> videoIds)
//        {
//            if (track != null) return;

//            track = catalogTracks.FirstOrDefault(Func);
//            if (track != default(SearchResult))
//            {
//                videoIds.Add(track.VideoId);

//                //track.Ordinal = "=";
//            }
//        }

//        public static void Fa1(this List<SearchResult> searchResults, string prefix, ref SearchResult track, 
//                               List<string> artists, string[] kind, string[] version, string[] otherVersion, List<string> preferredChannelIds,
//                               List<string> videoIds)
//        {
//            foreach (string artist in artists)
//            {
//                string artist1 = artist.ToLower();
//                string artist2 = artist1.Replace(" ", "");
//                //string artistvevo = artist1 + "VEVO".ToLower();

//                searchResults.Filter(prefix + artist + " 1", ref track, s => s.ChannelTitle.Eq(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 2", ref track, s => s.ChannelTitle.Eq(artist1) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 3", ref track, s => s.ChannelTitle.Eq(artist2) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 4", ref track, s => s.ChannelTitle.Eq(artist2) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 5", ref track, s => s.ChannelTitle._IsVevo(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 6", ref track, s => s.ChannelTitle._IsVevo(artist1) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 7", ref track, s => s.ChannelTitle._IsVevoLike(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 8", ref track, s => s.ChannelTitle._IsVevoLike(artist1) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//            }

//            searchResults.Filter(prefix + "9", ref track, s => preferredChannelIds.Contains(s.ChannelId) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//            searchResults.Filter(prefix + "10", ref track, s => preferredChannelIds.Contains(s.ChannelId) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//            foreach (string artist in artists)
//            {
//                string artist1 = artist.ToLower();
//                string artist2 = artist1.Replace(" ", "");

//                searchResults.Filter(prefix + artist + " 11", ref track, s => s.ChannelTitle._StartsWith(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 12", ref track, s => s.ChannelTitle._StartsWith(artist1) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 13", ref track, s => s.ChannelTitle._StartsWith(artist2) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//                searchResults.Filter(prefix + artist + " 14", ref track, s => s.ChannelTitle._StartsWith(artist2) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//            }
//        }

//        public static void Fa3(this List<SearchResult> searchResults, string prefix, ref SearchResult track, 
//                               List<string> artists, string[] kind, string[] version, string[] otherVersion, List<string> preferredChannelIds,
//                               List<string> videoIds)
//        {
//            foreach (string artist in artists)
//            {
//                string artist1 = artist.ToLower();
//                string artist2 = artist1.Replace(" ", "");
//                //string artistvevo = artist1 + "VEVO".ToLower();

//                searchResults.Filter(prefix + artist + " 1", ref track, s => s.ChannelTitle.Eq(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 3", ref track, s => s.ChannelTitle.Eq(artist2) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 5", ref track, s => s.ChannelTitle._IsVevo(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 7", ref track, s => s.ChannelTitle._IsVevoLike(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//            }

//            searchResults.Filter(prefix + "9", ref track, s => preferredChannelIds.Contains(s.ChannelId) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//            foreach (string artist in artists)
//            {
//                string artist1 = artist.ToLower();
//                string artist2 = artist1.Replace(" ", "");

//                searchResults.Filter(prefix + artist + " 11", ref track, s => s.ChannelTitle._StartsWith(artist1) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);

//                searchResults.Filter(prefix + artist + " 13", ref track, s => s.ChannelTitle._StartsWith(artist2) && s.Title._ContainsAny(kind) && s.Title._ContainsAny(version) && s.Title._NotContains(otherVersion), videoIds);
//            }
//        }

//        //private void Filter(string Rule, Func<Common.Models.SearchResult, bool> Func)
//        //{
//        //    if (!found)
//        //    {
//        //        foreach (SearchResult searchResult in SearchResults)
//        //        {
//        //            if (Func.Invoke(searchResult))
//        //            {
//        //                found = true;
//        //                newVideoIds.Add(searchResult.VideoId);
//        //                //CurrentSong.sVideoID = searchResult.Id.VideoId;
//        //                youtubeResultItem.FoundTrack = searchResult;
//        //                youtubeResultItem.Rule = Rule;
//        //                break;
//        //            }
//        //        }
//        //    }
//        //}
//    }
//}
