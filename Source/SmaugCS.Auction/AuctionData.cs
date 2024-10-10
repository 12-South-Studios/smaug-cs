using SmaugCS.Data.Instances;

namespace SmaugCS.Auction;

public class AuctionData
{
    public ObjectInstance ItemForSale { get; set; }
    public CharacterInstance Seller { get; set; }
    public CharacterInstance Buyer { get; set; }
    public int BidAmount { get; set; }
    public int GoingCounter { get; set; }
    public float PulseFrequency { get; set; }
    public int StartingBid { get; set; }
}