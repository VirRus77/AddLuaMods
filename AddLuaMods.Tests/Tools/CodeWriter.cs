using System.Text;

namespace AddLuaMods.Tests.Tools
{
    public class CodeWriter
    {
        private readonly char _ident;
        private readonly int _identSize;
        private readonly StringBuilder _builder;
        private int _currentIdent = 0;

        public CodeWriter(char ident = ' ', int identSize = 4)
        {
            _ident = ident;
            _identSize = identSize;
            _builder = new StringBuilder();
        }

        public int CurrentIdent => _currentIdent;

        public void Write(string line = "", bool ident = false)
        {
            if (ident && _identSize > 0 && _currentIdent > 0)
            {
                _builder.Append(GetCurrentIdent());
            }

            _builder.Append(line);
        }

        public void WriteLine(string line = "", bool ident = false)
        {
            if (ident && _identSize > 0 && _currentIdent > 0)
            {
                _builder.Append(GetCurrentIdent());
            }

            _builder.AppendLine(line);
        }

        public void IncrementIdent()
        {
            ++_currentIdent;
        }

        public void DecrementIdent()
        {
            --_currentIdent;
        }

        public void Clear()
        {
            _builder.Clear();
            _currentIdent = 0;
        }

        public string GetIdent(int ident)
        {
            return string.Empty.PadLeft(ident * _identSize, _ident);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        private string GetCurrentIdent()
        {
            return GetIdent(_currentIdent);
        }
    }
}
