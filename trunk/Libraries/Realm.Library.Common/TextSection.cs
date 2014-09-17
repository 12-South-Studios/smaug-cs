using System.Collections.Generic;

namespace Realm.Library.Common
{
    public class TextSection
    {
        public string Header { get; set; }

        private readonly List<string> _lines;

        public IEnumerable<string> Lines
        {
            get { return _lines; }
        }

        public string Footer { get; set; }

        public TextSection()
        {
            _lines = new List<string>();
        }

        public void AddLine(string line)
        {
            _lines.Add(line);
        }
    }
}
