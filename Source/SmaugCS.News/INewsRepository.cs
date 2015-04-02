using System.Collections.Generic;

namespace SmaugCS.News
{
    public interface INewsRepository
    {
        void Add(NewsData news);
        void Load();
        void Save();

        IEnumerable<NewsData> News { get; }
    }
}
