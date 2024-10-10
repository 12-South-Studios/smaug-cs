using SmaugCS.Data.Instances;

namespace SmaugCS.Auction;

public interface IAuctionManager
{
    void Initialize();
    void Save();
    AuctionData StartAuction(CharacterInstance seller, ObjectInstance item, int startingPrice);
    void PlaceBid(CharacterInstance bidder, int bidAmount);
    void StopAuction();

    AuctionData Auction { get; }
    IAuctionRepository Repository { get; }
}