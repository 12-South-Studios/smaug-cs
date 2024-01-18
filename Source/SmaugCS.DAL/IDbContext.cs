using Realm.Library.Common.Objects;
using SmaugCS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.DAL
{
    public interface IDbContext : IDisposable
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class, IEntity;
        Task<TEntity> GetAsync<TEntity>(long id) where TEntity : class, IEntity;
        Task<TEntity> AddOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;
        Task<long> CountAsync<TEntity>() where TEntity : class, IEntity;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IEntity;

        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class, IEntity;
        TEntity Get<TEntity>(long id) where TEntity : class, IEntity;
        TEntity AddOrUpdate<TEntity>(TEntity entity) where TEntity : class, IEntity;
        long Count<TEntity>() where TEntity : class, IEntity;
        void Delete<TEntity>(TEntity entity) where TEntity: class, IEntity;


        //Task<IEnumerable<Property>> GetPropertiesAsync();
        //Task<Property> GetPropertyAsync(string name);
        //Task<Property> AddOrUpdatePropertyAsync(Property property);
        //Task ClearPropertiesAsync();

        //Task<IEnumerable<News>> GetNewsAsync();
        //Task<News> GetNewsAsync(string name);
        //Task<News> AddOrUpdateNewsAsync(News news);

        //Task<Account> GetAccountAsync(string value);
        //Task<Account> GetAccountAsync(Guid id);
        //Task<Account> AddOrUpdateAccountAsync(Account account);

        //Task<IEnumerable<Character>> GetCharactersAsync(Guid accountId);
        //Task<Character> GetCharacterAsync(Guid id);
        //Task<Character> GetCharacterAsync(string name);
        //Task<Character> AddOrUpdateCharacterAsync(Character character);
        //// todo Task DeleteCharacterAsync(Character character);
        //Task<IEnumerable<Character>> GetCharactersByAccountAsync(Guid id);

        //Task<IEnumerable<Auction>> GetAuctionsAsync();
        //Task<IEnumerable<Auction>> GetCharacterAuctionsAsync(Guid characterId);
        //Task<IEnumerable<Auction>> GetAccountAuctionsAsync(Guid accountId);
        //Task<IEnumerable<AuctionBid>> GetCharacterAuctionBidsAsync(Guid characterId);
        //Task<IEnumerable<AuctionBid>> GetAccountAuctionBidsAsync(Guid accountId);
        //Task<Auction> AddOrUpdateAuctionAsync(Auction auction);
        //Task<Auction> GetAuctionAsync(Guid id);
        //Task<AuctionBid> AddOrUpdateAuctionBidAsync(AuctionBid bid);
        //Task<AuctionBid> GetAuctionBidAsync(Guid id);

        //Task<IEnumerable<Guild>> GetGuildsAsync();
        //Task<Guild> GetGuildAsync(Guid id);
        //Task<Guild> GetGuildAsync(string name);
        //Task<Guild> AddOrUpdateGuildAsync(Guild guild);
        //Task<GuildChannel> AddOrUpdateGuildChannel(Guid guildId, GuildChannel channel);
        //Task<GuildMember> AddOrUpdateGuildMember(Guid guildId, GuildMember member);
        //Task<GuildUpgrade> AddOrUpdateGuildUpgrade(Guid guildId, GuildUpgrade upgrade);
    }
}
