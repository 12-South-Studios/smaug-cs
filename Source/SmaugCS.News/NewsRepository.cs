using SmaugCS.DAL;
using SmaugCS.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.News
{
    public class NewsRepository : INewsRepository
    {
        public IEnumerable<NewsData> News { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IDbContext _dbContext;

        public NewsRepository(ILogManager logManager, IDbContext dbContext)
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
                if (_dbContext.Count<DAL.Models.News>() == 0) return;

                foreach (DAL.Models.News news in _dbContext.GetAll<DAL.Models.News>())
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

                    foreach (DAL.Models.NewsEntry entry in news.Entries)
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

                    _dbContext.AddOrUpdate<DAL.Models.News>(newsToSave);
                }
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }
    }
}
