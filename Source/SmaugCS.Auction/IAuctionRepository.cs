using System.Collections.Generic;

namespace SmaugCS.Auction;

public interface IAuctionRepository
{
    void Add(AuctionData auction);
    void Load();
    void Save();

    IEnumerable<AuctionHistory> History { get; }
}