using System.Collections.Generic;
using System.Linq;

namespace SmaugCS.Data
{
    public class AuctionData
    {
        public ObjectInstance ItemForSale { get; set; }
        public CharacterInstance Seller { get; set; }
        public CharacterInstance Buyer { get; set; }
        public int BidAmount { get; set; }
        public int GoingCounter { get; set; }
        public float PulseFrequency { get; set; }
        public int StartingBid { get; set; }

        public IEnumerable<ObjectTemplate> History { get; private set; }
        public short hist_timer { get; set; }

        public AuctionData(int auctionMem)
        {
            History = new List<ObjectTemplate>();
        }

        public void AddToHistory(ObjectTemplate template)
        {
            History.ToList().Add(template);
        }

        public void AddToHistory(ObjectInstance instance)
        {
            AddToHistory(instance.ObjectIndex);
        }
    }
}
