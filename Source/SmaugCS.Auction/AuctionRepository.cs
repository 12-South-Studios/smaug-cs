using SmaugCS.DAL;
using SmaugCS.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.Auction;

// ReSharper disable once ClassNeverInstantiated.Global
public class AuctionRepository(ILogManager logManager, IDbContext dbContext) : IAuctionRepository
{
  public IEnumerable<AuctionHistory> History { get; private set; } = [];

  public void Add(AuctionData auction)
  {
    AuctionHistory history = new()
    {
      BuyerName = auction.Buyer.Name,
      SellerName = auction.Seller.Name,
      ItemForSale = auction.ItemForSale.ObjectIndex.Id,
      SoldFor = auction.BidAmount,
      SoldOn = DateTime.UtcNow,
      Saved = false
    };
    History.ToList().Add(history);
  }

  public void Load()
  {
    try
    {
      if (dbContext.Count<DAL.Models.Auction>() == 0) return;

      List<AuctionHistory> auctions = dbContext.GetAll<DAL.Models.Auction>().Select(auction => new AuctionHistory
      {
        BuyerName = auction.BuyerName,
        ItemForSale = auction.ItemSoldId,
        SellerName = auction.SellerName,
        SoldFor = auction.SoldFor,
        SoldOn = auction.SoldOn
      }).ToList();

      History = auctions.ToList();
      logManager.Boot("Loaded {0} Auctions", History.Count());
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }

  public void Save()
  {
    try
    {
      foreach (AuctionHistory history in History.Where(x => !x.Saved).ToList())
      {
        DAL.Models.Auction auction = new()
        {
          BuyerName = history.BuyerName,
          ItemSoldId = history.ItemForSale,
          SellerName = history.SellerName,
          SoldFor = history.SoldFor,
          SoldOn = history.SoldOn
        };
        dbContext.AddOrUpdate(auction);
        history.Saved = true;
      }
    }
    catch (DbException ex)
    {
      logManager.Error(ex);
      throw;
    }
  }
}