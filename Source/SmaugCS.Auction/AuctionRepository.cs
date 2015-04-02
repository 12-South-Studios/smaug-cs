using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SmaugCS.DAL.Interfaces;
using SmaugCS.Logging;

namespace SmaugCS.Auction
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuctionRepository : IAuctionRepository
    {
        public IEnumerable<AuctionHistory> History { get; private set; }
        private readonly ILogManager _logManager;
        private readonly ISmaugDbContext _dbContext;

        public AuctionRepository(ILogManager logManager, ISmaugDbContext dbContext)
        {
            History = new List<AuctionHistory>();
            _logManager = logManager;
            _dbContext = dbContext;
        }

        public void Add(AuctionData auction)
        {
            var history = new AuctionHistory
            {
                BuyerName = auction.Buyer.Name,
                SellerName = auction.Seller.Name,
                ItemForSale = auction.ItemForSale.ObjectIndex.ID,
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
                foreach (var history in _dbContext.Auctions.Select(auction => new AuctionHistory
                {
                    BuyerName = auction.BuyerName,
                    ItemForSale = auction.ItemSoldId,
                    SellerName = auction.SellerName,
                    SoldFor = auction.SoldFor,
                    SoldOn = auction.SoldOn
                }))
                {
                    History.ToList().Add(history);
                }
                _logManager.Boot("Loaded {0} Auctions", History.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }

        public void Save()
        {
            try
            {
                foreach (var history in History.Where(x => !x.Saved).ToList())
                {
                    var auction = _dbContext.Auctions.Create();
                    auction.BuyerName = history.BuyerName;
                    auction.ItemSoldId = history.ItemForSale;
                    auction.SellerName = history.SellerName;
                    auction.SoldFor = history.SoldFor;
                    auction.SoldOn = history.SoldOn;
                    history.Saved = true;
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
            }
        }
    }
}
