using System;
using System.Collections.Generic;

namespace Common
{
    public static class ListExtension
    {
        public static T Previous<T>(this List<T> list, int startIndex, Func<T, bool> predicate)
        {
            for (int i = startIndex; i >= 0; i--)
            {
                if (predicate.Invoke(list[i]))
                    return list[i];
            }

            return default(T);
        }

        public static T Next<T>(this List<T> list, int startIndex, Func<T, bool> predicate)
        {
            for (int i = startIndex; i < list.Count; i++)
            {
                if (predicate.Invoke(list[i]))
                    return list[i];
            }

            return default(T);
        }

        public static bool _Contains(this List<string> list, string element)
        {
            foreach (string item in list)
            {
                if (item.Eq(element)) return true;
            }

            return false;
        }

        public static bool _ContainsMatch(this List<string> list, string element)
        {
            foreach (string item in list)
            {
                if (item._Match(element)) return true;
            }

            return false;
        }

        public static bool _ContainsWordMatchAny(this List<string> list, string element)
        {
            foreach (string item in list)
            {
                if (item._WordMatchAny(element)) return true;
            }

            return false;
        }

        public static bool _ContainsSubstring(this List<string> list, string s)
        {
            foreach (string item in list)
            {
                if (item.ToLower().Contains(s.ToLower())) return true;
            }

            return false;
        }
    }
}