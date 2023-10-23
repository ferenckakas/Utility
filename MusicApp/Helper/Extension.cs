using System;
using System.Collections.Generic;
using System.Linq;

namespace Helper
{
    public static class Extension
    {
        public static void Filter(this List<CatalogTrack> catalogTracks, string id, ref CatalogTrack track, Func<CatalogTrack, bool> Func, List<string> songIds)
        {
            if (track != null) return;

            track = catalogTracks.FirstOrDefault(Func);
            if (track != default(CatalogTrack))
            {
                songIds.Add(track.CatalogId);
                track.Ordinal = $"={id}";
            }
        }

        public static void Filter(this List<CatalogTrack> catalogTracks, string id, ref CatalogTrack track, Func<CatalogTrack, bool> Func, List<string> songIds, uint? duration)
        {
            if (track != null) return;

            if (duration != null && catalogTracks.Any(t => t.TotalTime != null && t.TotalTime > 0))
            {
                List<CatalogTrack> tracks = catalogTracks.Where(Func).ToList();

                if (tracks.Count() > 0)
                {
                    int minIndex = 0;
                    int min = Math.Abs((int)(tracks[minIndex].TotalTime - duration));
                    for (var i = 1; i < tracks.Count(); i++)
                    {
                        int d = Math.Abs((int)(tracks[i].TotalTime - duration));
                        if (d < min)
                        {
                            min = d;
                            minIndex = i;
                        }
                    }

                    track = tracks[minIndex];
                    songIds.Add(track.CatalogId);
                    track.Ordinal = $"={id}";
                }
            }
        }
        public static void Filter(this List<YoutubeTrack> youtubeTracks, string id, ref YoutubeTrack track, Func<YoutubeTrack, bool> Func, List<string> videoIds, Track song = null)
        {
            if (track != null) return;

            if (song != null && song.TotalTime.HasValue && youtubeTracks.Any(t => t.DurationInMilliSec > 0))
            {
                int duration = song.TotalTime.Value;

                List<YoutubeTrack> tracks = youtubeTracks.Where(Func).ToList();

                if (tracks.Count() > 0)
                {
                    int minIndex = 0;
                    int min = Math.Abs(tracks[minIndex].DurationInMilliSec - duration);
                    for (var i = 1; i < tracks.Count(); i++)
                    {
                        int d = Math.Abs(tracks[i].DurationInMilliSec - duration);
                        if (d < min)
                        {
                            min = d;
                            minIndex = i;
                        }
                    }

                    track = tracks[minIndex];
                    videoIds.Add(track.VideoId);
                    track.Ordinal = $"={id}";
                }
            }
            else if (youtubeTracks.Any(t => t.Year.HasValue))
            {
                List<YoutubeTrack> tracks = youtubeTracks.Where(Func).ToList();

                if (tracks.Count() > 0)
                {
                    int minIndex = 0;
                    uint min = tracks[minIndex].Year.HasValue ? tracks[minIndex].Year.Value : uint.MaxValue;
                    for (var i = 1; i < tracks.Count(); i++)
                    {
                        uint d = tracks[i].Year.HasValue ? tracks[i].Year.Value : uint.MaxValue;
                        if (d < min)
                        {
                            min = d;
                            minIndex = i;
                        }
                    }

                    track = tracks[minIndex];
                    videoIds.Add(track.VideoId);
                    track.Ordinal = $"={id}";
                }
            }
            else
            {
                track = youtubeTracks.FirstOrDefault(Func);
                if (track != default(YoutubeTrack))
                {
                    videoIds.Add(track.VideoId);
                    track.Ordinal = $"={id}";
                }
            }
        }

        //public static void Filter2(this List<SearchResult> catalogTracks, ref SearchResult track, Func<SearchResult, bool> Func, List<string> videoIds)
        //{
        //    if (track != null) return;

        //    track = catalogTracks.FirstOrDefault(Func);
        //    if (track != default(SearchResult))
        //    {
        //        videoIds.Add(track.VideoId);

        //        //track.Ordinal = "=";
        //    }
        //}

        //private void Filter(string Rule, Func<Common.Models.SearchResult, bool> Func)
        //{
        //    if (!found)
        //    {
        //        foreach (SearchResult searchResult in SearchResults)
        //        {
        //            if (Func.Invoke(searchResult))
        //            {
        //                found = true;
        //                newVideoIds.Add(searchResult.VideoId);
        //                //CurrentSong.sVideoID = searchResult.Id.VideoId;
        //                youtubeResultItem.FoundTrack = searchResult;
        //                youtubeResultItem.Rule = Rule;
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
