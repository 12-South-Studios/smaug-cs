using System;
using System.Collections.Generic;

namespace SmaugCS.News
{
    public class NewsData
    {
        private readonly List<NewsEntryData> _entries;
 
        public int Id { get; private set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }
        public bool Saved { get; set; }

        public IEnumerable<NewsEntryData> Entries
        {
            get { return _entries; }
        }

        public NewsData(int id)
        {
            Id = id;
            _entries = new List<NewsEntryData>();
            Saved = false;
        }

        public void AddEntry(NewsEntryData entry)
        {
            if (!_entries.Contains(entry))
                _entries.Add(entry);
        }
    }
}
