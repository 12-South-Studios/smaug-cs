using System.Collections.Generic;

namespace SmaugCS.News
{
    public class NewsData
    {
        public List<NewsEntryData> news { get; set; }
        public string header { get; set; }
        public string cmd_name { get; set; }
        public string name { get; set; }
        public int vnum { get; set; }
        public short level { get; set; }
    }
}
