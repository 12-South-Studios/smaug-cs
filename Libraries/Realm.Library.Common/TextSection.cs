using System.Collections.Generic;

namespace Realm.Library.Common
{
    public class TextSection
    {
        public string Header { get; set; }
        public IEnumerable<string> Lines { get; private set; }
        public string Footer { get; set; }

        public TextSection()
        {
            Lines = new List<string>();
        }
    }
}
