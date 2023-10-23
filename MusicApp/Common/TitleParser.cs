using System;

namespace Common
{
    public static class TitleParser
    {
        public static string GetArtist(string title)
        {
            string[] sep = { " - " };
            string[] parts = title.Split(sep, StringSplitOptions.None);
            return parts[0];
        }

        public static string GetTitle(string title)
        {
            string[] sep = { " - " };
            string[] parts = title.Split(sep, StringSplitOptions.None);
            return parts.Length > 1 ? parts[1] : "";
        }
    }
}