using System;
using System.Collections.Generic;

namespace SmaugCS.News
{
    public class NewsData
    {
        private readonly List<NewsEntryData> _entries;
 
        public int Id { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }

        public IEnumerable<NewsEntryData> Entries
        {
            get { return _entries; }
        }

        public NewsData()
        {
            _entries = new List<NewsEntryData>();
        }

        public void AddEntry(NewsEntryData entry)
        {
            if (!_entries.Contains(entry))
                _entries.Add(entry);
        }
    }
}
