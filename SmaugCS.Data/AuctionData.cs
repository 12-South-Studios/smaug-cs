using SmaugCS.Data;
using SmaugCS.Data;

namespace SmaugCS.Data
{
    public class AuctionData
    {
        public ObjectInstance ItemForSale { get; set; }
        public CharacterInstance Seller { get; set; }
        public CharacterInstance Buyer { get; set; }
        public int CoinAmount { get; set; }
        public short going { get; set; }
        public short pulse { get; set; }
        public int starting { get; set; }
        public ObjectTemplate[] history { get; set; }
        public short hist_timer { get; set; }

        public AuctionData(int auctionMem)
        {
            history = new ObjectTemplate[auctionMem];
        }
    }
}
