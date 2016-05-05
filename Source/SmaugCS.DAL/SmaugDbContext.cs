using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Realm.Library.Common.Logging;
using SmaugCS.DAL.Interfaces;
using SmaugCS.DAL.Models;

namespace SmaugCS.DAL
{
    internal class SmaugDbContext : DbContext, ISmaugDbContext
    {
        private ILogWrapper Logger { get; }
        public ObjectContext ObjectContext { get; }

        private DateTime _lastSaveTimeUtc;

        public SmaugDbContext() : base("Smaug")
        {
            ObjectContext = ((IObjectContextAdapter)this).ObjectContext;
        }
        public SmaugDbContext(ILogWrapper logger) : base("Smaug")
        {
            Logger = logger;
            ObjectContext = ((IObjectContextAdapter) this).ObjectContext;
        }

        public override Task<int> SaveChangesAsync()
        {
            _lastSaveTimeUtc = DateTime.UtcNow;

            ProcessEntities(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            return base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            _lastSaveTimeUtc = DateTime.UtcNow;

            ProcessEntities(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            try
            {
               return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                LogDbEntityValidationResults(e);
                throw;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        private void LogDbEntityValidationResults(DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                var error =
                    $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:";

                Console.WriteLine(error);
                Logger.Error(error);

                foreach (var ve in eve.ValidationErrors)
                {
                    var validationError = $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"";

                    Console.WriteLine(validationError);
                    Logger.ErrorFormat(validationError);
                }
            }
        }

        private void ProcessEntities(Func<DbEntityEntry, bool> predicate)
        {
            foreach (var entry in ChangeTracker.Entries().Where(predicate))
            {
                var entity = entry.Entity as IEntity;
                if (entity == null) continue;

                AddCreateDateToEntity(entry);
            }
        }

        private void AddCreateDateToEntity(DbEntityEntry entry)
        {
            var entity = entry.Entity as IEntity;
            if (entity == null) return;

            if (entry.State == EntityState.Modified && !entity.CreateDateUtc.HasValue)//Migration entities don't have create date.
                entity.CreateDateUtc = entry.OriginalValues.GetValue<DateTime>("CreateDateUtc");
            
            if (entry.State == EntityState.Added)
                entity.CreateDateUtc = _lastSaveTimeUtc;

            if (!entity.CreateDateUtc.HasValue)
            {
                throw new InvalidOperationException(
                    $"Create date for object doesn't exist. This is required.  Object type: {entry.Entity.GetType().Name}");
            }
        }

        public IDbSet<Auction> Auctions { get; set; }
        public IDbSet<Ban> Bans { get; set; }
        public IDbSet<Board> Boards { get; set; }
        public IDbSet<Character> Characters { get; set; }
        public IDbSet<GameState> GameStates { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<News> News { get; set; }
        public IDbSet<NewsEntry> NewsEntries { get; set; }
        public IDbSet<Note> Notes { get; set; } 
        public IDbSet<Organization> Organizations { get; set; }
        public IDbSet<WeatherCell> Weather { get; set; }
        public IDbSet<Session> Sessions { get; set; } 
    }
}