using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.News
{
    public class NewsRepository : INewsRepository
    {
        public IEnumerable<NewsData> News { get; private set; }
        private readonly ILogManager _logManager;
        private readonly ISmaugDbContext _dbContext;

        public NewsRepository(ILogManager logManager, ISmaugDbContext dbContext)
        {
            News = new List<NewsData>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(NewsData news)
        {
            News.ToList().Add(news);
        }

        public void Load()
        {
            try
            {
                foreach (var b in _dbContext.News)
                {
                    var newNews = new NewsData(b.Id)
                    {
                        Name = b.Name,
                        Header = b.Header,
                        Level = b.Level,
                        CreatedBy = b.CreatedBy,
                        CreatedOn = b.CreatedOn,
                        Active = b.IsActive
                    };
                    News.ToList().Add(newNews);

                    foreach (var n in b.Entries)
                    {
                        var newEntry = new NewsEntryData
                        {
                            Id = n.Id,
                            Title = n.Title,
                            Name = n.Name,
                            Text = n.Text,
                            PostedOn = n.PostedOn,
                            PostedBy = n.PostedBy,
                            Active = n.IsActive
                        };
                        newNews.Entries.ToList().Add(newEntry);
                    }
                }
                _logManager.Boot("Loaded {0} News", News.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }

        public void Save()
        {
            try
            {
                foreach (var news in News.Where(x => !x.Saved).ToList())
                {
                    var newsToSave = _dbContext.News.Create();
                    newsToSave.CreatedBy = news.CreatedBy;
                    newsToSave.CreatedOn = news.CreatedOn;
                    newsToSave.Header = news.Header;
                    newsToSave.IsActive = news.Active;
                    newsToSave.Level = news.Level;
                    newsToSave.Name = news.Name;
                    news.Saved = true;

                    foreach (var entry in news.Entries.Where(y => !y.Saved).ToList())
                    {
                        var entryToSave = _dbContext.NewsEntries.Create();
                        entryToSave.IsActive = entry.Active;
                        entryToSave.Name = entry.Name;
                        entryToSave.NewsId = news.Id;
                        entryToSave.PostedBy = entry.PostedBy;
                        entryToSave.PostedOn = entry.PostedOn;
                        entryToSave.Title = entry.Title;
                        entryToSave.Text = entry.Text;
                        entry.Saved = true;
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }
    }
}
