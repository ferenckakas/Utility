using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringParser
{
    public static class StringExtension
    {
        public static string RemoveHTML(this string sText)
        {
            string s = sText;

            Str str = new Str(s);

            while (str.Find("<"))
            {
                s = str.DeleteTo(">");
                str = new Str(s);
            }

            s = s.Replace(">", "");
            return s;
        }

        public static string RemovePars(this string sText)
        {
            int iStart = sText.IndexOf('(');
            if (iStart > -1)
            {
                int iEnd = sText.LastIndexOf(')');
                if (iEnd > -1)
                {
                    sText = sText.Remove(iStart, iEnd - iStart + 1);
                }
                else
                {
                    sText = sText.Remove(iStart);
                }
            }

            return sText.Trim();
        }

        public static string RemoveBrackets(this string sText)
        {
            int iStart = sText.IndexOf('[');
            if (iStart > -1)
            {
                int iEnd = sText.LastIndexOf(']');
                if (iEnd > -1)
                {
                    sText = sText.Remove(iStart, iEnd - iStart + 1);
                }
                else
                {
                    sText = sText.Remove(iStart);
                }
            }

            return sText.Trim();
        }

        public static string RemoveParsIfNotContains(this string sText, params string[] words)
        {
            int iStart = sText.IndexOf('(');
            if (iStart > -1)
            {
                bool result = false;
                string sub;

                int iEnd = sText.LastIndexOf(')');

                if (iEnd > -1)
                    sub = sText.Substring(iStart, iEnd - iStart + 1).ToLower();
                else
                    sub = sText.Substring(iStart).ToLower();

                foreach (string word in words)
                {
                    result = sub.Contains(word.ToLower());
                    if (result == true) break;
                }

                if (result == false)
                {
                    if (iEnd > -1)
                        sText = sText.Remove(iStart, iEnd - iStart + 1);
                    else
                        sText = sText.Remove(iStart);
                }
            }

            return sText.Trim();
        }

        public static string GetVersion(this string sText)
        {
            string a = sText.GetVersion('(', ')');
            if (a == "") a = sText.GetVersion('[', ']');
            return a;
        }

        public static string GetVersion(this string sText, char c1, char c2)
        {
            int iStart = sText.IndexOf(c1);
            int iEnd;

            while (iStart > -1)
            {
                string sub;

                iEnd = sText.IndexOf(c2, iStart + 1);

                if (iEnd > -1)
                    sub = sText.Substring(iStart, iEnd - iStart + 1).ToLower();
                else
                    sub = sText.Substring(iStart).ToLower();

                if (sub.Contains("(remix") || sub.Contains(" remix") ||
                    sub.Contains("(mix") || sub.Contains(" mix") ||
                    sub.Contains(" version") || sub.Contains(" edit"))
                {
                    if (iEnd > -1)
                        return sub.Substring(1, sub.Length - 2);
                    else
                        return sub.Substring(1, sub.Length - 1);
                }

                if (iEnd > -1)
                    iStart = sText.IndexOf(c1, iEnd + 1);
                else
                    iStart = -1;
            }

            return "";
        }

        public static string GetArtists(this string sText)
        {
            string a = sText.GetArtists('(', ')');
            if (a == "") a = sText.GetArtists('[', ']');
            return a;
        }

        public static string GetArtists(this string sText, char c1, char c2)
        {
            int iStart = sText.IndexOf(c1);
            int iEnd;

            while (iStart > -1)
            {
                string sub;

                iEnd = sText.IndexOf(c2, iStart + 1);

                if (iEnd > -1)
                    sub = sText.Substring(iStart, iEnd - iStart + 1).ToLower();
                else
                    sub = sText.Substring(iStart).ToLower();

                List<string> feat = new List<string> { "feat. ", "feat ", "featuring ", "ft. ", "ft " };

                int la;
                if (iEnd > -1) la = 2;
                else la = 1;

                foreach (var f in feat)
                {
                    if (sub.Contains(f)) return sub.Substring(1 + f.Length, sub.Length - f.Length - la);
                }

                if (iEnd > -1)
                    iStart = sText.IndexOf(c1, iEnd + 1);
                else
                    iStart = -1;
            }

            return "";
        }

        public static string RemoveAccents(this string a)
        {
            return a.Replace("á", "a").Replace("é", "e").Replace("í", "i").
                     Replace("ó", "o").Replace("ö", "o").Replace("ő", "o").
                     Replace("ú", "u").Replace("ü", "u").Replace("ű", "u").
                     Replace("ø", "o");
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
                if (Char.IsLetterOrDigit(c))
                    newString += c;
            }

            return newString;
        }

        public static bool _IsVevo(this string a, string b)
        {
            Dictionary<string, string> Exceptions = new Dictionary<string, string>()
            {
                { "nicki minaj", "NickiMinajAtVEVO" },
                { "rage against the machine", "RATMVEVO" },
                { "bring me the horizon", "BMTHOfficialVEVO" },
                { "swedish house mafia", "SHMVEVO" },
                { "psy", "officialpsy" }
            };

            if (Exceptions.ContainsKey(b))
            {
                return a == Exceptions[b];
            }
            else
            {
                a = a.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").RemoveAccents().RemoveSigns();
                a = Regex.Replace(a, "^the", "", RegexOptions.IgnoreCase);

                b = b.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").Replace(" ", "").RemoveAccents().RemoveSigns() + "VEVO".ToLower();

                return a == b;
            }
        }

        public static bool _IsVevoLike(this string a, string b)
        {
            a = a.Trim().ToLower().Replace("the ", "");
            a = Regex.Replace(a, "^the", "", RegexOptions.IgnoreCase);

            b = b.Trim().Replace("&", "and").Replace("+", "and").Replace(" x ", " and ").ToLower().Replace("the ", "").Replace(" ", "").RemoveAccents().RemoveSigns();

            return a.StartsWith(b) && a.EndsWith("vevo");
        }

        public static bool _StartsWith(this string a, string b)
        {
            a = a.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();
            b = b.Trim().StandardizeApostrophe().Replace(".", "").Replace("!", "").ToLower().RemoveAccents();

            return a.StartsWith(b);
        }

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
    }
}
