using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringParser
{
    public class Str
    {
        string sText;
        int iPos;
        string sLastPattern;

        public int Pos { get { return iPos; } }

        public Str(string sText)
        {
            this.sText = sText;
            this.iPos = 0;
            this.sLastPattern = "";
        }

        public void Init()
        {
            this.iPos = 0;
            this.sLastPattern = "";
        }

        public bool Find(string sPattern)
        {
            sLastPattern = sPattern;
            int iNewPos = sText.IndexOf(sPattern, iPos);
            if (iNewPos > -1) iPos = iNewPos;
            return iNewPos > -1;
        }

        public bool Skip()
        {
            if (sLastPattern != "")
            {
                iPos = iPos + sLastPattern.Length;
                return true;
            }
            else
                return false;
        }

        public Str Inner(string sEnd)
        {
            this.Skip(); this.Find(">"); this.Skip();

            string sResult = this.Read(sEnd);

            return new Str(sResult);
        }
        public string Read(string sEnd)
        {
            sLastPattern = sEnd;

            int iEnd = sText.IndexOf(sEnd, iPos);

            if (iEnd > -1)
            {
                string sResult = sText.Substring(iPos, iEnd - iPos);
                iPos = iEnd;
                return sResult;
            }
            else
            {
                sLastPattern = "";
                return "";
            }
        }

        public string Read(int length)
        {
            string sResult = sText.Substring(iPos, length);
            sLastPattern = sResult;
            //iPos += length;
            return sResult;
        }

        public string ReadTo(string sEnd)
        {
            sLastPattern = sEnd;

            int iEnd = sText.IndexOf(sEnd, iPos);

            if (iEnd > -1)
            {
                iEnd += sEnd.Length;
                return sText.Substring(iPos, iEnd - iPos);
            }
            else
                return "";
        }

        public bool Found(string sPattern)
        {
            //sLastPattern = sPattern;
            return sText.IndexOf(sPattern, iPos) > -1;
        }

        public string ReadToEnd()
        {
            return sText.Substring(iPos);
        }

        public int IndexOf(string sPattern)
        {
            return sText.IndexOf(sPattern, iPos);
        }

        public string Delete(string sEnd)
        {
            sLastPattern = sEnd;

            int iEnd = sText.IndexOf(sEnd, iPos);

            return sText.Remove(iPos, iEnd - iPos);
        }

        public string DeleteTo(string sEnd)
        {
            sLastPattern = sEnd;

            int iEnd = sText.IndexOf(sEnd, iPos);
            if (iEnd > -1)
                iEnd += sEnd.Length;
            else
                iEnd = sText.Length - 1;

            sText = sText.Remove(iPos, iEnd - iPos);
            return sText;
        }

        public string ReplaceTo(string sEnd, string sNewString)
        {
            sLastPattern = sEnd;

            int iEnd = sText.IndexOf(sEnd, iPos);
            iEnd += sEnd.Length;

            sText = sText.Remove(iPos, iEnd - iPos);
            sText = sText.Insert(iPos, sNewString);
            return sText;
        }

        //public string HtmlDecode()
        //{
        //    return HttpUtility.HtmlDecode(sText);
        //}
    }
}
