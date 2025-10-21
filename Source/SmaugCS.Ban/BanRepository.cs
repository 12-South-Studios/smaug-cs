using SmaugCS.DAL;
using SmaugCS.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.Ban;

// ReSharper disable once ClassNeverInstantiated.Global
public class BanRepository(ILogManager logManager, IDbContext dbContext) : IBanRepository
{
  public IEnumerable<BanData> Bans { get; } = [];

  public void Add(BanData ban) => Bans.ToList().Add(ban);

  public void Load()
  {
    try
    {
      if (dbContext.Count<DAL.Models.Ban>() == 0) return;

      IEnumerable<DAL.Models.Ban> bans = dbContext.GetAll<DAL.Models.Ban>();
      foreach (BanData newBan in bans.Select(ban => new BanData
               {
                 Id = ban.Id,
                 Type = ban.BanType,
                 BannedBy = ban.BannedBy,
                 BannedOn = ban.BannedOn,
                 Duration = ban.Duration,
                 Level = ban.Level,
                 Name = ban.Name,
                 Note = ban.Note,
                 Prefix = ban.IsPrefix,
                 Suffix = ban.IsSuffix
               }))
      {
        Bans.ToList().Add(newBan);
      }

      logManager.Boot("Loaded {0} Bans", Bans.Count());
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
      foreach (BanData ban in Bans.Where(x => !x.Saved).ToList())
      {
        DAL.Models.Ban banToSave = new()
        {
          BannedBy = ban.BannedBy,
          BannedOn = ban.BannedOn,
          Duration = ban.Duration,
          Level = ban.Level,
          BanType = ban.Type,
          IsPrefix = ban.Prefix,
          IsSuffix = ban.Suffix,
          Name = ban.Name,
          Note = ban.Note
        };
        ban.Saved = true;
        dbContext.AddOrUpdate(banToSave);
      }
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }

  public void Delete(int id)
  {
    try
    {
      BanData localBan = Bans.FirstOrDefault(x => x.Id == id);
      if (localBan == null) return;
      Bans.ToList().Remove(localBan);

      DAL.Models.Ban ban = dbContext.Get<DAL.Models.Ban>(id);
      if (ban == null) return;

      dbContext.Delete(ban);
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }
}