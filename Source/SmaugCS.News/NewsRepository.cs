using SmaugCS.DAL;
using SmaugCS.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Models;

namespace SmaugCS.News;

public class NewsRepository(ILogManager logManager, IDbContext dbContext) : INewsRepository
{
  public IEnumerable<NewsData> News { get; private set; } = new List<NewsData>();

  public void Add(NewsData news)
  {
    News.ToList().Add(news);
  }

  public void Load()
  {
    try
    {
      if (dbContext.Count<DAL.Models.News>() == 0) return;

      foreach (DAL.Models.News news in dbContext.GetAll<DAL.Models.News>())
      {
        NewsData newNews = new(news.Id)
        {
          Name = news.Name,
          Header = news.Header,
          Level = news.Level,
          CreatedBy = news.CreatedBy,
          CreatedOn = news.CreatedOn,
          Active = news.IsActive
        };
        News.ToList().Add(newNews);

        foreach (NewsEntry entry in news.Entries)
        {
          NewsEntryData newEntry = new()
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

      logManager.Boot("Loaded {0} News", News.Count());
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }

  public void Save()
  {
    try
    {
      foreach (NewsData news in News.Where(x => !x.Saved).ToList())
      {
        DAL.Models.News newsToSave = new()
        {
          CreatedBy = news.CreatedBy,
          CreatedOn = news.CreatedOn,
          Header = news.Header,
          IsActive = news.Active,
          Level = news.Level,
          Name = news.Name
        };
        news.Saved = true;

        foreach (NewsEntryData entry in news.Entries.Where(y => !y.Saved).ToList())
        {
          NewsEntry entryToSave = new()
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

        dbContext.AddOrUpdate(newsToSave);
      }
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }
}