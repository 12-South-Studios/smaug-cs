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
                if (!_dbContext.News.Any()) return;

                foreach (DAL.Models.News news in _dbContext.News)
                {
                    var newNews = new NewsData(news.Id)
                    {
                        Name = news.Name,
                        Header = news.Header,
                        Level = news.Level,
                        CreatedBy = news.CreatedBy,
                        CreatedOn = news.CreatedOn,
                        Active = news.IsActive
                    };
                    News.ToList().Add(newNews);

                    foreach(DAL.Models.NewsEntry entry in news.Entries) 
                    {
                        var newEntry = new NewsEntryData
                        {
                            Id = entry.Id,
                            Title = entry.Title,
                            Name = entry.Name,
                            Text = entry.Text,
                            PostedOn = entry.PostedOn,
                            PostedBy = entry.PostedBy,
                            Active = entry.IsActive
                        };
                        newNews.Entries.ToList().Add(newEntry);
                    }
                }
                _logManager.Boot("Loaded {0} News", News.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        public void Save()
        {
            try
            {
                foreach (var news in News.Where(x => !x.Saved).ToList())
                {
                    var newsToSave = new DAL.Models.News
                    {
                        CreatedBy = news.CreatedBy,
                        CreatedOn = news.CreatedOn,
                        Header = news.Header,
                        IsActive = news.Active,
                        Level = news.Level,
                        Name = news.Name
                    };
                    news.Saved = true;
                    _dbContext.News.Add(newsToSave);

                    foreach (var entry in news.Entries.Where(y => !y.Saved).ToList())
                    {
                        var entryToSave = new DAL.Models.NewsEntry
                        {
                            IsActive = entry.Active,
                            Name = entry.Name,
                            PostedBy = entry.PostedBy,
                            PostedOn = entry.PostedOn,
                            Title = entry.Title,
                            Text = entry.Text
                        };
                        entry.Saved = true;
                        newsToSave.Entries.Add(entryToSave);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }
    }
}
