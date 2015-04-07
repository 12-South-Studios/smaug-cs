using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Infrastructure.Data;
using SmaugCS.Logging;

namespace SmaugCS.Ban
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BanRepository : IBanRepository
    {
        public IEnumerable<BanData> Bans { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IRepository _repository;

        public BanRepository(ILogManager logManager, IRepository repository)
        {
            Bans = new List<BanData>();
            _logManager = logManager;
            _repository = repository;
        }

        public void Add(BanData ban)
        {
            Bans.ToList().Add(ban);
        }

        public void Load()
        {
            try
            {
                foreach (var newBan in _repository.GetQuery<DAL.Models.Ban>().Select(ban => new BanData(ban.Id, ban.BanType)
                {
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
            }
        }

        public void Save()
        {
            try
            {
                _repository.UnitOfWork.BeginTransaction();
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
                    _repository.Attach(banToSave);
                }
                _repository.UnitOfWork.CommitTransaction();
            }
            catch (DbException ex)
            {
                _repository.UnitOfWork.RollBackTransaction();
                _logManager.Error(ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var localBan = Bans.FirstOrDefault(x => x.Id == id);
                if (localBan == null) return;
                Bans.ToList().Remove(localBan);

                var ban = _repository.GetQuery<DAL.Models.Ban>().FirstOrDefault(x => x.Id == id);
                if (ban == null) return;

                _repository.UnitOfWork.BeginTransaction();
                _repository.Delete(ban);
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
