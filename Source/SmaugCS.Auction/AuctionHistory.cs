using System;

namespace SmaugCS.Auction;

public class AuctionHistory
{
    public string BuyerName { get; set; }
    public string SellerName { get; set; }
    public long ItemForSale { get; set; }
    public DateTime SoldOn { get; set; }
    public int SoldFor { get; set; }
    public bool Saved { get; set; }
}