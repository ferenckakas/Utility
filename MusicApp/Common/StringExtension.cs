using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common
{
    public static class StringExtension
    {
        public static uint? ToNullableUInt(this string s)
        {
            uint output;
            return uint.TryParse(s, out output) ? (uint?)output : null;
        }

        public static string RemoveHTML(this string text)
        {
            string s = text;

            var str = new Str(s);

            while (str.Find("<"))
            {
                s = str.DeleteTo(">");
                str = new Str(s);
            }

            s = s.Replace(">", "");
            return s;
        }

        public static string RemovePars(this string text)
        {
            int startIndex = text.IndexOf('(');
            if (startIndex > -1)
            {
                int endIndex = text.LastIndexOf(')');
                if (endIndex > -1)
                {
                    text = text.Remove(startIndex, endIndex - startIndex + 1);
                }
                else
                {
                    text = text.Remove(startIndex);
                }
            }

            return text.Trim();
        }

        public static string RemoveBrackets(this string text)
        {
            int startIndex = text.IndexOf('[');
            if (startIndex > -1)
            {
                int endIndex = text.LastIndexOf(']');
                if (endIndex > -1)
                {
                    text = text.Remove(startIndex, endIndex - startIndex + 1);
                }
                else
                {
                    text = text.Remove(startIndex);
                }
            }

            return text.Trim();
        }

        public static string RemoveParsIfNotContains(this string text, params string[] words)
        {
            int startIndex = text.IndexOf('(');
            if (startIndex > -1)
            {
                bool result = false;
                string sub;

                int endIndex = text.LastIndexOf(')');

                if (endIndex > -1)
                    sub = text.Substring(startIndex, endIndex - startIndex + 1).ToLower();
                else
                    sub = text.Substring(startIndex).ToLower();

                foreach (string word in words)
                {
                    result = sub.Contains(word.ToLower());
                    if (result == true) break;
                }

                if (result == false)
                {
                    if (endIndex > -1)
                        text = text.Remove(startIndex, endIndex - startIndex + 1);
                    else
                        text = text.Remove(startIndex);
                }
            }

            return text.Trim();
        }

        public static string GetVersion(this string text)
        {
            string a = text.GetVersion('(', ')');
            if (a == "") a = text.GetVersion('[', ']');
            return a;
        }

        public static string GetVersion(this string text, char c1, char c2)
        {
            int startIndex = text.IndexOf(c1);
            int endIndex;

            while (startIndex > -1)
            {
                string sub;

                endIndex = text.IndexOf(c2, startIndex + 1);

                if (endIndex > -1)
                    sub = text.Substring(startIndex, endIndex - startIndex + 1).ToLower();
                else
                    sub = text.Substring(startIndex).ToLower();

                if (sub.Contains("(remix") || sub.Contains(" remix") ||
                    sub.Contains("(mix") || sub.Contains(" mix") ||
                    sub.Contains(" version") || sub.Contains(" edit"))
                {
                    if (endIndex > -1)
                        return sub.Substring(1, sub.Length - 2);
                    else
                        return sub.Substring(1, sub.Length - 1);
                }

                if (endIndex > -1)
                    startIndex = text.IndexOf(c1, endIndex + 1);
                else
                    startIndex = -1;
            }

            return "";
        }

        public static string GetArtists(this string text)
        {
            string a = text.GetArtists('(', ')');
            if (a == "") a = text.GetArtists('[', ']');
            return a;
        }

        public static string GetArtists(this string text, char c1, char c2)
        {
            int startIndex = text.IndexOf(c1);
            int endIndex;

            while (startIndex > -1)
            {
                string sub;

                endIndex = text.IndexOf(c2, startIndex + 1);

                if (endIndex > -1)
                    sub = text.Substring(startIndex, endIndex - startIndex + 1).ToLower();
                else
                    sub = text.Substring(startIndex).ToLower();

                List<string> feat = new List<string> { "feat. ", "feat ", "featuring ", "ft. ", "ft " };

                int la;
                if (endIndex > -1) la = 2;
                else la = 1;

                foreach (string f in feat)
                {
                    if (sub.Contains(f)) return sub.Substring(1 + f.Length, sub.Length - f.Length - la);
                }

                if (endIndex > -1)
                    startIndex = text.IndexOf(c1, endIndex + 1);
                else
                    startIndex = -1;
            }

            return "";
        }

        public static string RemoveAccents(this string a)
        {
            return a.Replace("á", "a").Replace("é", "e").Replace("í", "i").
                        Replace("ó", "o").Replace("ö", "o").Replace("ő", "o").
                        Replace("ú", "u").Replace("ü", "u").Replace("ű", "u").
                        Replace("ø", "o").Replace("å", "a").Replace("ñ", "n");
        }

        public static string StandardizeApostrophe(this string a)
        {
            return a.Replace("'", "’").Replace("`", "’").Replace("’", "");
        }

        public static bool Eq(this string a, string b)
        {
            a = a.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").RemoveAccents();
            b = b.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").RemoveAccents();

            return a == b;
        }

        public static bool VEq(this string a, string b)
        {
            //if (a == null && b == null)
            //    return true;

            //if (a == null || b == null)
            //    return false;

            a = a.Trim().ToLower().RemoveAccents();
            b = b.Trim().ToLower().RemoveAccents();

            return a == b;
        }

        public static bool _Match(this string a, string b)
        {
            //if (a == null && b == null)
            //    return true;

            //if (a == null || b == null)
            //    return false;

            a = a.Trim().ToLower().ToAlphaNum().RemoveMultipleSpaces().RemoveAccents();
            b = b.Trim().ToLower().ToAlphaNum().RemoveMultipleSpaces().RemoveAccents();

            return a == b;
        }

        public static bool _StartsWith(this string a, string b)
        {
            //if (a == null && b == null)
            //    return true;

            //if (a == null || b == null)
            //    return false;

            // keep the lenght
            a = a.ToLower().ToAlphaNumOrReplace().RemoveAccents();
            b = b.ToLower().ToAlphaNumOrReplace().RemoveAccents();

            return a.StartsWith(b);
        }


        public static bool _YEq(this string a, string b)
        {
            a = a.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").RemoveAccents();
            a = Regex.Replace(a, "^the", "", RegexOptions.IgnoreCase);

            b = b.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").Replace(" ", "").RemoveAccents();

            return a == b;
        }

        public static string RemoveSigns(this string a)
        {
            string newString = "";
            foreach (char c in a)
            {
                if (!char.IsSymbol(c))
                    newString += c;
            }

            return newString;
        }

        //public static bool _IsVevo(this string a, string b)
        //{
        //    Dictionary<string, string> Exceptions = new Dictionary<string, string>()
        //    {
        //        { "nicki minaj", "NickiMinajAtVEVO" },
        //        { "rage against the machine", "RATMVEVO" },
        //        { "bring me the horizon", "BMTHOfficialVEVO" },
        //        { "swedish house mafia", "SHMVEVO" },
        //        { "psy", "officialpsy" }
        //    };

        //    if (Exceptions.ContainsKey(b))
        //    {
        //        return a == Exceptions[b];
        //    }
        //    else
        //    {
        //        a = a.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").RemoveAccents().RemoveSigns();
        //        a = Regex.Replace(a, "^the", "", RegexOptions.IgnoreCase);

        //        b = b.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").Replace(" ", "").RemoveAccents().RemoveSigns() + "VEVO".ToLower();

        //        return a == b;
        //    }
        //}

        public static bool _IsVevoLike(this string a, string b)
        {
            a = a.Trim().ToLower().Replace("the ", "");
            a = Regex.Replace(a, "^the", "", RegexOptions.IgnoreCase);

            b = b.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").Replace(" ", "").RemoveAccents().RemoveSigns();

            return a.StartsWith(b) && a.EndsWith("vevo");
        }

        //public static bool _StartsWith(this string a, string b)
        //{
        //    a = a.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();
        //    b = b.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();

        //    return a.StartsWith(b);
        //}

        public static bool _EndsWith(this string a, string b)
        {
            a = a.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();
            b = b.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();

            return a.EndsWith(b);
        }

        public static bool _Contains(this string a, string b)
        {
            a = a.Trim().StandardizeApostrophe().ToLower().RemoveAccents();
            b = b.Trim().StandardizeApostrophe().ToLower().RemoveAccents();

            return a.Contains(b);
        }

        public static bool _ContainsAny(this string a, params string[] bs)
        {
            bool result = false;

            a = a.Trim().StandardizeApostrophe().ToLower().RemoveAccents();

            foreach (string b in bs)
            {
                string b2 = b.StandardizeApostrophe().ToLower().RemoveAccents();
                result = a.Contains(b2);
                if (result == true) break;
            }

            return result;
        }

        public static bool _NotContains(this string a, params string[] bs)
        {
            bool result = false;

            a = a.Trim().StandardizeApostrophe().ToLower().RemoveAccents();

            foreach (string b in bs)
            {
                string b2 = b.StandardizeApostrophe().ToLower().RemoveAccents();
                result = !a.Contains(b2);
                if (result == false) break;
            }

            return result;
        }

        public static bool IsNumber(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            else
            {
                foreach (char c in s)
                {
                    if (!char.IsDigit(c))
                        return false;
                }

                return true;
            }
        }

        public static bool _WordMatchMutual(this string a, string b)
        {
            a = a.ToLower().ToAlphaNum().RemoveAccents();
            b = b.ToLower().ToAlphaNum().RemoveAccents();

            List<string> aList = a._ToWords();
            List<string> bList = b._ToWords();

            bool oka = aList.All(a1 => bList.Contains(a1));
            bool okb = bList.All(b1 => aList.Contains(b1));

            if (oka && okb) return true;

            return false;
        }

        public static bool _WordMatchAny(this string a, string b)
        {
            a = a.ToLower().ToAlphaNum().RemoveAccents();
            b = b.ToLower().ToAlphaNum().RemoveAccents();

            List<string> aList = a._ToWords();
            List<string> bList = b._ToWords();

            bool oka = aList.All(a1 => bList.Contains(a1));
            //bool oka = aArray.Any(a1 => !bArray.Contains(a1));

            if (oka) return true;

            bool okb = bList.All(b1 => aList.Contains(b1));

            if (okb) return true;

            return false;
        }

        public static bool _WordMatchPartial(this string a, string b)
        {
            a = a.ToLower().ToAlphaNum().RemoveAccents();
            b = b.ToLower().ToAlphaNum().RemoveAccents();

            List<string> aList = a._ToWords();
            List<string> bList = b._ToWords();

            bool oka = aList.Any(a1 => bList.Contains(a1));

            if (oka) return true;

            bool okb = bList.Any(b1 => aList.Contains(b1));

            if (okb) return true;

            return false;
        }

        public static string ToEnglishChars(this string s)
        {
            string newString = "";

            if (string.IsNullOrEmpty(s))
            {
                return newString;
            }

            foreach (char c in s)
            {
                if (Constant.EnglishAlphabet.Contains(c) || c == ' ')
                    newString = $"{newString}{c}";
            }

            return newString;
        }

        //public static string ToLetters(this string s)
        //{
        //    string newString = "";

        //    if (string.IsNullOrEmpty(s))
        //    {
        //        return newString;
        //    }

        //    string englishAlphabet = "abcdefghijklmnopqrstuvwxyz";

        //    foreach (char c in s)
        //    {
        //        if (char.is || c == ' ')
        //            newString = $"{newString}{c}";
        //    }

        //    return newString;
        //}

        public static string ToAlphaNum(this string a)
        {
            string newString = "";
            foreach (char c in a)
            {
                if (char.IsLetterOrDigit(c) || c == ' ')
                    newString += c;
            }

            return newString;
        }

        public static string ToAlphaNumOrReplace(this string a)
        {
            string newString = "";
            foreach (char c in a)
            {
                if (char.IsLetterOrDigit(c) || c == ' ')
                    newString += c;
                else
                    newString += " ";
            }

            return newString;
        }

        public static string RemoveMultipleSpaces(this string a)
        {
            string newString = "";
            bool isPrevSpace = false;
            for (var i = 0; i < a.Length; i++)
            {
                char c = a[i];
                if (c == ' ')
                {
                    if (!isPrevSpace)
                    {
                        newString += c;
                        isPrevSpace = true;
                    }
                }
                else
                {
                    newString += c;
                    isPrevSpace = false;
                }
            }

            return newString;
        }

        public static string ReplaceMultipleSpaces(this string a)
        {
            string newString = "";
            bool isPrevSpace = false;
            for (var i = 0; i < a.Length; i++)
            {
                char c = a[i];
                if (c == ' ')
                {
                    if (!isPrevSpace)
                    {
                        newString += c;
                        isPrevSpace = true;
                    }
                    else
                        newString += "0";
                }
                else
                {
                    newString += c;
                    isPrevSpace = false;
                }
            }

            return newString;
        }

        public static List<string> Capture(this string a, string pattern)
        {
            var captures = new List<string>();

            MatchCollection matches = Regex.Matches(a, pattern);

            for (var i = 0; i < matches.Count; i++)
            {
                string value = matches[i].Groups[1].Value;
                captures.Add(value);
            }

            return captures;
        }

        public static List<string> CaptureBrackets(this string a)
        {
            List<string> list1 = a.Capture(@"\(([^()]*)\)");   // (xxx)
            List<string> list2 = a.Capture(@"\[([^\[\]]*)\]"); // [yyy]
            list1.AddRange(list2);

            return list1;
        }

        public static List<string> _ToWords(this string a)
        {
            return a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> _ToWords2(this string a)
        {
            string word = "";
            List<string> words = new List<string>();
            foreach (char c in a)
            {
                if (char.IsLetter(c))
                    word = $"{word}{c}";
                else
                {
                    if (!string.IsNullOrWhiteSpace(word)) { words.Add(word); word = ""; }
                    words.Add(c.ToString());
                }
            }

            if (!string.IsNullOrWhiteSpace(word)) words.Add(word);

            return words;
        }

        public static List<string> _SplitByTab(this string a)
        {
            return a.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static List<string> _SplitByPipe(this string a)
        {
            return a.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToList();
        }

        //public List<string> SubWords(this List<string> words, int startIndex, int lenght)
        //{


        //    throw new NotImplementedException();
        //}


        private static readonly List<string> _songVersionList = new List<string>()
        { "mix", "remix", "dancemix", "extended", "edit", "version", "cut", "mashup", "reboot", "cover", "instrumental", "karaoke", "live", "acoustic", "beyond the video", "élő" };

        private static readonly List<string> _songVersionExcludeList = new List<string>()
        { "remastered", "single", "radio", "album" };

        private static readonly List<string> _featList = new List<string>()
        { "featuring", "feat.", "feat", "ft.", "ft", "with", "km.", "km" };

        public static string ParseFeat(this string a)
        {
            List<string> list = a.CaptureBrackets();

            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _featList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    if (index > -1)
                    {
                        return string.Join(" ", words.GetRange(index + 1, words.Count - index - 1));
                    }
                }
            }

            return null;
        }

        public static string ParseSongVersion(this string a)
        {
            List<string> list = a.CaptureBrackets();

            for (var i = 0; i < list.Count; i++)
            {
                List<string> words = list[i]._ToWords();
                foreach (string f in _songVersionList)
                {
                    List<string> wordslowerCase = words.Select(w => w.ToLower()).ToList();
                    int index = wordslowerCase.IndexOf(f);
                    int indexExclude = -1;
                    _songVersionExcludeList.ForEach(e => { if (indexExclude == -1) indexExclude = wordslowerCase.IndexOf(e); });

                    if (index > -1 && indexExclude == -1)
                    {
                        return string.Join(" ", words);
                    }
                }
            }

            return null;
        }

        public static string RemoveAndFeat(this string a)
        {
            return a.Replace(", ", " ").Replace(" ,", " ").Replace(",", " ")
                    .Replace(" & ", " ")
                    .Replace(" + ", " ")
                    .Replace(" and ", " ").Replace(" And ", " ").Replace(" AND ", " ")
                    .Replace(" with ", " ").Replace(" With ", " ").Replace(" WITH ", " ")
                    .Replace(" x ", " ").Replace(" X ", " ")
                    .Replace(" vs. ", " ").Replace(" vs.", " ").Replace(" vs ", " ")
                    .Replace(" Vs. ", " ").Replace(" Vs.", " ").Replace(" Vs ", " ")
                    .Replace(" VS. ", " ").Replace(" VS.", " ").Replace(" VS ", " ")
                    .Replace(" featuring ", " ")
                    .Replace(" Featuring ", " ")
                    .Replace(" FEATURING ", " ")
                    .Replace(" feat. ", " ").Replace(" feat.", " ").Replace(" feat ", " ")
                    .Replace(" Feat. ", " ").Replace(" Feat.", " ").Replace(" Feat ", " ")
                    .Replace(" FEAT. ", " ").Replace(" FEAT.", " ").Replace(" FEAT ", " ")
                    .Replace(" ft. ", " ").Replace(" ft.", " ").Replace(" ft ", " ")
                    .Replace(" Ft. ", " ").Replace(" Ft.", " ").Replace(" Ft ", " ")
                    .Replace(" FT. ", " ").Replace(" FT.", " ").Replace(" FT ", " ")
                    .Replace(" km. ", " ").Replace(" km.", " ").Replace(" km ", " ")
                    .Replace(" Km. ", " ").Replace(" Km.", " ").Replace(" Km ", " ")
                    .Replace(" KM. ", " ").Replace(" KM.", " ").Replace(" KM ", " ");
        }

        public static string TrimQuots(this string a)
        {
            if (string.IsNullOrWhiteSpace(a))
                return a;

            var quotList = new List<string>() { "'", "`", "’", "‘", "\"", "”", "“" };

            if (quotList.Contains(a.Substring(0, 1)) && quotList.Contains(a.Substring(a.Length - 1, 1)))
            {
                string b = a.Substring(1);
                return b.Substring(0, b.Length - 1);
            }

            return a;
        }

        public static string ToSetUpperFirstChar(this string a)
        {
            return $"{a[0].ToString().ToUpper()}{a.Substring(1)}";
        }

        public static string ToUpperFirstChar(this string a)
        {
            return $"{a[0].ToString().ToUpper()}{a.Substring(1).ToLower()}";
        }

        public static string ToHungarianTitleCase(this string a)
        {
            return $"{a[0].ToString().ToUpper()}{a.Substring(1).ToLower()}";
        }
    }
}
