using System;
using System.Collections.Generic;

namespace SmaugCS.News
{
    public class NewsData
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Active { get; set; }
        public List<NewsEntryData> Entries { get; set; }

        public NewsData()
        {
            Entries = new List<NewsEntryData>();
        }
    }
}
