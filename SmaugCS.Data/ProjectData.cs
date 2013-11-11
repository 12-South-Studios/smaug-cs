using System.Collections.Generic;

namespace SmaugCS.Data
{
    public class ProjectData
    {
        public List<NoteData> Logs { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public string coder { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public bool taken { get; set; }

        public ProjectData()
        {
            Logs = new List<NoteData>();
        }

        public NoteData GetLog(int num)
        {
            int count = 0;
            foreach (NoteData note in Logs)
            {
                count++;
                if (count == num)
                    return note;
            }
            return null;
        }
    }
}
