using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Realm.Library.Common.Objects;
using SmaugCS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmaugCS.DAL.Exceptions;

namespace SmaugCS.DAL
{
    public partial class DbContext : Realm.Library.Common.Objects.Entity, IDbContext
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private IMongoDatabase _database;

        public IMongoDatabase Database
        {
            get
            {
                if (_database != null) { return _database; }

                var client = new MongoClient(_connectionString);
                _database = client.GetDatabase(MongoUrl.Create(_connectionString).DatabaseName);
                return _database;
            }
        }

        private IMongoCollection<Character> Characters { get { return Database.GetCollection<Character>(CollectionNames.Characters); } }
        private IMongoCollection<News> News { get { return Database.GetCollection<News>(CollectionNames.News); } }
        private IMongoCollection<Auction> Auctions { get { return Database.GetCollection<Auction>(CollectionNames.Auctions); } }
        private IMongoCollection<Clan> Clans { get { return Database.GetCollection<Clan>(CollectionNames.Clans); } }
        private IMongoCollection<Ban> Bans { get { return Database.GetCollection<Ban>(CollectionNames.Bans); } }
        private IMongoCollection<Board> Boards { get { return Database.GetCollection<Board>(CollectionNames.Boards); } }
        private IMongoCollection<Organization> Organizations { get { return Database.GetCollection<Organization>(CollectionNames.Organizations); } }
        private IMongoCollection<Log> Logs {  get { return Database.GetCollection<Log>(CollectionNames.Logs); } }
        private IMongoCollection<WeatherCell> Weather {  get {  return Database.GetCollection<WeatherCell>(CollectionNames.Weather); } }
        private IMongoCollection<GameState> GameStates {  get { return Database.GetCollection<GameState>(CollectionNames.GameStates); } }
        private IMongoCollection<Session> Sessions {  get {  return Database.GetCollection<Session>(CollectionNames.Sessions); } }

        public DbContext(long id, IConfiguration configuration, ILogger<DbContext> logger) : base(id, typeof(DbContext).FullName)
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            _connectionString = connectionString;
            _logger = logger;
            Registrations();
        }

        private IMongoCollection<T> GetMongoCollection<T>() where T : class, IEntity
        {
            var type = typeof(T);
            if (type == typeof(Character)) return (IMongoCollection<T>)Characters;
            if (type == typeof(News)) return (IMongoCollection<T>)News;
            if (type == typeof(Auction)) return (IMongoCollection<T>)Auctions;
            if (type == typeof(Clan)) return (IMongoCollection<T>)Clans;
            if (type == typeof(Ban)) return (IMongoCollection<T>)Bans;
            if (type == typeof(Board)) return (IMongoCollection<T>)Boards;
            if (type == typeof(Organization)) return (IMongoCollection<T>)Organizations;
            if (type == typeof(Log)) return (IMongoCollection<T>)Logs;
            if (type == typeof(WeatherCell)) return (IMongoCollection<T>)Weather;
            if (type == typeof(GameState)) return (IMongoCollection<T>)GameStates;
            if (type == typeof(Session)) return (IMongoCollection<T>)Sessions;
            return null;
        }

        private static readonly List<Type> KnownClassTypes = new List<Type>()
        {
            typeof(Auction), typeof(Ban), typeof(Board), typeof(Character), typeof(Clan), typeof(News), typeof(Note), typeof(Organization)
        };

        private void Registrations()
        {
            KnownClassTypes.ForEach(t =>
            {
                if (BsonClassMap.IsClassMapRegistered(t)) return;

                var bsonClassMap = new BsonClassMap(t);
                bsonClassMap.AutoMap();
                bsonClassMap.SetDiscriminator(t.FullName);
                BsonClassMap.RegisterClassMap(bsonClassMap);
            });

            ProcessSpecialMapping();
        }

        private static void ProcessSpecialMapping()
        {
            //// Auction
            //if (!BsonClassMap.IsClassMapRegistered(typeof(AuctionBid)))
            //{
            //    BsonClassMap.RegisterClassMap<AuctionBid>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Auction);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(AuctionItem)))
            //{
            //    BsonClassMap.RegisterClassMap<AuctionItem>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Auction);
            //    });
            //}


            //// Character
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterBank)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterBank>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterBankItem)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterBankItem>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Bank);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterChannel)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterChannel>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterEffect)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterEffect>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterFormula)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterFormula>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterItem)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterItem>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterMail)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterMail>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterMailItem)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterMailItem>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Letter);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterMemory)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterMemory>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterProfession)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterProfession>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterQuest)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterQuest>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterRitual)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterRitual>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}
            //if (!BsonClassMap.IsClassMapRegistered(typeof(CharacterSkill)))
            //{
            //    BsonClassMap.RegisterClassMap<CharacterSkill>(cm =>
            //    {
            //        cm.AutoMap();
            //        cm.UnmapMember(m => m.Character);
            //    });
            //}


        }

        #region Health Check
        private static Tuple<string, string> _databaseInfo;
        public virtual Tuple<string, string> GetDatabaseInfo()
        {
            if (_databaseInfo != null) return _databaseInfo;

            var url = MongoUrl.Create(_connectionString);
            _databaseInfo = new Tuple<string, string>(url.Server.Host, url.DatabaseName);
            return _databaseInfo;
        }

        public virtual async Task<string> CheckConnectionAsync()
        {
            try
            {
                var result = await Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return result == null ? "Could not connect" : "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Base Methods
        public virtual IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public virtual T Get<T>(Guid id, string collectionName) where T : Models.Entity
        {
            var collection = Database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Where(x => x.Id == id);

            var results = collection.Find(filter);
            return results.FirstOrDefault();
        }
        public virtual T Get<T>(FilterDefinition<T> filter, string collectionName) where T : Models.Entity
        {
            var collection = Database.GetCollection<T>(collectionName);

            var results = collection.Find(filter);
            return results.FirstOrDefault();
        }

        public virtual IMongoQueryable<T> GetList<T>(string collectionName) where T : Models.Entity
        {
            var collection = Database.GetCollection<T>(collectionName);
            return collection.AsQueryable();
        }

        #endregion

        #region Additional CRUD Async Methods
        public virtual async Task<long> CountAsync<TEntity>() where TEntity : class, IEntity
        {
            try
            {
                var collection = GetMongoCollection<TEntity>();
                if (collection == null) return 0;

                return await collection.CountDocumentsAsync(_ => true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new DbException(ex.Message, ex);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class, IEntity
        {
            try
            {
                var collection = GetMongoCollection<TEntity>();
                if (collection == null) return new List<TEntity>();

                return await (await collection.FindAsync(_ => true)).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new DbException(ex.Message, ex);
            }
        }

        public virtual async Task<TEntity> GetAsync<TEntity>(long id) where TEntity : class, IEntity
        {
            try
            {
                var collection = GetMongoCollection<TEntity>();
                if (collection == null) return null;

                var filter = Builders<TEntity>.Filter.Where(x => x.Id == id);
                var results = await collection.FindAsync(filter);
                return await results.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new DbException(ex.Message, ex);
            }
        }
        public virtual async Task<TEntity> AddOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            try
            {
                var collection = GetMongoCollection<TEntity>();
                if (collection == null) return null;

                var filter = Builders<TEntity>.Filter.Where(x => x.Id == entity.Id);
                var results = await collection.FindAsync(filter);

                if (!results.Any())
                {
                    await collection.InsertOneAsync(entity);
                }
                else
                {
                    await collection.ReplaceOneAsync(filter, entity);
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new DbException(ex.Message, ex);
            }
        }
        public virtual async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            try
            {
                var collection = GetMongoCollection<TEntity>();
                if (collection == null) return;

                var filter = Builders<TEntity>.Filter.Where(x => x.Id == entity.Id);
                var results = await collection.DeleteOneAsync(filter);

                if (results.DeletedCount > 1)
                {
                    throw new Exception($"Deleted {results.DeletedCount} records!");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new DbException(ex.Message, ex);
            }
        }
        #endregion

        #region Additional CRUD Sync Methods
        public virtual IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, IEntity
        {
            return Task.Run(async () => await GetAllAsync<TEntity>()).Result;
        }
        public virtual TEntity Get<TEntity>(long id) where TEntity : class, IEntity
        {
            return Task.Run(async () => await GetAsync<TEntity>(id)).Result;
        }
        public virtual TEntity AddOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            return Task.Run(async () => await AddOrUpdateAsync<TEntity>(entity)).Result;    
        }
        public virtual long Count<TEntity>() where TEntity : class, IEntity
        {
            return Task.Run(async () => await CountAsync<TEntity>()).Result;
        }
        public virtual void Delete<TEntity>(TEntity entity) where TEntity: class, IEntity
        {
            Task.Run(async () => await DeleteAsync<TEntity>(entity));
        }
        #endregion
    }
}
