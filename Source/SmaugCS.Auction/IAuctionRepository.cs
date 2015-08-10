using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmaugCS.Auction
{
    public interface IAuctionRepository
    {
        void Add(AuctionData auction);
        void Load();
        Task Save();

        IEnumerable<AuctionHistory> History { get; }
    }
}
