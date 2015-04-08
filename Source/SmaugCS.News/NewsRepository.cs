using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Infrastructure.Data;
using SmaugCS.Logging;

namespace SmaugCS.News
{
    public class NewsRepository : INewsRepository
    {
        public IEnumerable<NewsData> News { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IRepository _repository;

        public NewsRepository(ILogManager logManager, IRepository repository)
        {
            News = new List<NewsData>();
            _logManager = logManager;
            _repository = repository;
        }

        public void Add(NewsData news)
        {
            News.ToList().Add(news);
        }

        public void Load()
        {
            try
            {
                foreach (var news in _repository.GetQuery<DAL.Models.News>())
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

                    foreach (var n in _repository.GetQuery<DAL.Models.NewsEntry>().Where(x => x.NewsId == newNews.Id))
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
                _repository.UnitOfWork.BeginTransaction();
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
                    _repository.Attach(newsToSave);

                    foreach (var entry in news.Entries.Where(y => !y.Saved).ToList())
                    {
                        var entryToSave = new DAL.Models.NewsEntry
                        {
                            IsActive = entry.Active,
                            Name = entry.Name,
                            NewsId = news.Id,
                            PostedBy = entry.PostedBy,
                            PostedOn = entry.PostedOn,
                            Title = entry.Title,
                            Text = entry.Text
                        };
                        entry.Saved = true;
                        _repository.Attach(entryToSave);
                    }
                }
                _repository.UnitOfWork.CommitTransaction();
            }
            catch (DbException ex)
            {
                _repository.UnitOfWork.RollBackTransaction();
                _logManager.Error(ex);
            }
        }
    }
}
