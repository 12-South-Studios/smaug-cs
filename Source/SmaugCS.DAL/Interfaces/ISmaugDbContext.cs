using SmaugCS.DAL.Models;
using System;
using System.Data.Entity;

namespace SmaugCS.DAL.Interfaces
{
    public interface ISmaugDbContext : IDisposable, ISmaugContext
    {
        IDbSet<Auction> Auctions { get; set; }
        IDbSet<Ban> Bans { get; set; }
        IDbSet<Board> Boards { get; set; }
        IDbSet<Character> Characters { get; set; }
        IDbSet<GameState> GameStates { get; set; }
        IDbSet<Log> Logs { get; set; }
        IDbSet<News> News { get; set; }
        IDbSet<NewsEntry> NewsEntries { get; set; }
        IDbSet<Note> Notes { get; set; }
        IDbSet<Organization> Organizations { get; set; }
        IDbSet<WeatherCell> Weather { get; set; }
        IDbSet<Session> Sessions { get; set; }
    }
}
