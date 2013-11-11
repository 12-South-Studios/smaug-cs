using System.Collections.Generic;

namespace SmaugCS.Data
{
    public class EditorData
    {
        public int NumberOfLines { get; set; }
        public int OnLine { get; set; }
        public int Size { get; set; }
        public List<string> Text { get; set; }

        public EditorData()
        {
            Text = new List<string>();
        }
    }
}
