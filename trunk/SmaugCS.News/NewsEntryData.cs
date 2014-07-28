using System;

namespace SmaugCS.News
{
    public class NewsEntryData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime PostedOn { get; set; }
        public string PostedBy { get; set; }
        public bool Active { get; set; }
    }
}
