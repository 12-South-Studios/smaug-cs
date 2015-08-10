using System.Threading.Tasks;
using SmaugCS.Data.Instances;

namespace SmaugCS.Auction
{
    public interface IAuctionManager
    {
        void Initialize();
        Task Save();
        AuctionData StartAuction(CharacterInstance seller, ObjectInstance item, int startingPrice);
        void PlaceBid(CharacterInstance bidder, int bidAmount);
        void StopAuction();

        AuctionData Auction { get; set; }
        IAuctionRepository Repository { get; }
    }
}
