using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyWebAPI.Test
{
    public static class StringExtension
    {
        public static bool Eq(this string a, string b)
        {
            a = a.Trim().Replace("'", "’").Replace(".", "").ToLower();
            b = b.Trim().Replace("'", "’").Replace(".", "").ToLower();

            return a == b;
        }

        public static bool _StartsWith(this string a, string b)
        {
            a = a.Trim().Replace("'", "’").Replace(".", "").ToLower();
            b = b.Trim().Replace("'", "’").Replace(".", "").ToLower();

            return a.StartsWith(b);
        }
        public static bool _Contains(this string a, string b)
        {
            a = a.Trim().Replace("'", "’").ToLower();
            b = b.Trim().Replace("'", "’").ToLower();

            return a.Contains(b);
        }

        public static bool _NotContains(this string a, params string[] bs)
        {
            bool result = false;

            a = a.Trim().Replace("'", "’").ToLower();

            foreach (string b in bs)
            {
                string b2 = b.Replace("'", "’").ToLower();
                result = !a.Contains(b2);
                if (result == false) break;
            }

            return result;
        }
    }
}