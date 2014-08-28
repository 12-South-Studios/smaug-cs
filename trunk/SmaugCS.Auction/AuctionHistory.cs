using System;
using System.Data;
using SmaugCS.Common;

namespace SmaugCS.Auction
{
    public class AuctionHistory
    {
        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public long ItemForSale { get; set; }
        public DateTime SoldOn { get; set; }
        public int SoldFor { get; set; }
        public bool Saved { get; set; }

        public static AuctionHistory Translate(DataRow dataRow)
        {
            AuctionHistory auction = new AuctionHistory
            {
                BuyerName = dataRow.GetDataValue("BuyerName", string.Empty),
                SellerName = dataRow.GetDataValue("SellerName", string.Empty),
                SoldOn = dataRow.GetDataValue("SoldOn", DateTime.MinValue),
                SoldFor = dataRow.GetDataValue("SoldFor", 0),
                ItemForSale = dataRow.GetDataValue("ItemSoldID", 0)
            };
            return auction;
        }
    }
}
