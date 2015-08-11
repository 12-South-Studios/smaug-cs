using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BanRepository : IBanRepository
    {
        public IEnumerable<BanData> Bans { get; private set; }
        private readonly ILogManager _logManager;
        private readonly ISmaugDbContext _dbContext;

        public BanRepository(ILogManager logManager, ISmaugDbContext dbContext)
        {
            Bans = new List<BanData>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(BanData ban)
        {
            Bans.ToList().Add(ban);
        }

        public void Load()
        {
            try
            {
                if (!_dbContext.Bans.Any()) return;

                foreach (var newBan in _dbContext.Bans.Select(ban => new BanData
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
                _logManager.Boot("Loaded {0} Bans", Bans.Count());
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
                foreach (var ban in Bans.Where(x => !x.Saved).ToList())
                {
                    var banToSave = new DAL.Models.Ban
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
                    _dbContext.Bans.Add(banToSave);
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var localBan = Bans.FirstOrDefault(x => x.Id == id);
                if (localBan == null) return;
                Bans.ToList().Remove(localBan);

                var ban = _dbContext.Bans.FirstOrDefault(x => x.Id == id);
                if (ban == null) return;

                _dbContext.Bans.Remove(ban);
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
