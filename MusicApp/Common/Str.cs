namespace Common
{
    public class Str
    {
        private string _text;
        private int _pos;
        private string _lastPattern;

        public int Pos { get { return _pos; } }

        public Str(string text)
        {
            _text = text;
            _pos = 0;
            _lastPattern = "";
        }

        public void Init()
        {
            _pos = 0;
            _lastPattern = "";
        }

        public bool Find(string pattern)
        {
            _lastPattern = pattern;
            int newPos = _text.IndexOf(pattern, _pos);
            if (newPos > -1) _pos = newPos;
            return newPos > -1;
        }

        public bool Skip()
        {
            if (_lastPattern != "")
            {
                _pos = _pos + _lastPattern.Length;
                return true;
            }
            else
                return false;
        }

        public Str Inner(string end)
        {
            Skip(); Find(">"); Skip();

            string result = Read(end);

            return new Str(result);
        }

        public string Read(string end)
        {
            _lastPattern = end;

            int index = _text.IndexOf(end, _pos);

            if (index > -1)
            {
                string result = _text.Substring(_pos, index - _pos);
                _pos = index;
                return result;
            }
            else
            {
                _lastPattern = "";
                return "";
            }
        }

        public string Read(int length)
        {
            string result = _text.Substring(_pos, length);
            _lastPattern = result;
            //_pos += length;
            return result;
        }

        public string ReadTo(string end)
        {
            _lastPattern = end;

            int index = _text.IndexOf(end, _pos);

            if (index > -1)
            {
                index += end.Length;
                return _text.Substring(_pos, index - _pos);
            }
            else
                return "";
        }

        public bool Found(string pattern)
        {
            //_lastPattern = pattern;
            return _text.IndexOf(pattern, _pos) > -1;
        }

        public string ReadToEnd()
        {
            return _text.Substring(_pos);
        }

        public int IndexOf(string pattern)
        {
            return _text.IndexOf(pattern, _pos);
        }

        public string Delete(string end)
        {
            _lastPattern = end;

            int index = _text.IndexOf(end, _pos);

            return _text.Remove(_pos, index - _pos);
        }

        public string DeleteTo(string end)
        {
            _lastPattern = end;

            int index = _text.IndexOf(end, _pos);
            if (index > -1)
                index += end.Length;
            else
                index = _text.Length - 1;

            _text = _text.Remove(_pos, index - _pos);
            return _text;
        }

        public string ReplaceTo(string sEnd, string sNewString)
        {
            _lastPattern = sEnd;

            int iEnd = _text.IndexOf(sEnd, _pos);
            iEnd += sEnd.Length;

            _text = _text.Remove(_pos, iEnd - _pos);
            _text = _text.Insert(_pos, sNewString);
            return _text;
        }

        //public string HtmlDecode()
        //{
        //    return HttpUtility.HtmlDecode(_text);
        //}
    }
}
