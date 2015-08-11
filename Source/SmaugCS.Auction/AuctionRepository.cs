using System;
using System.Collections.Generic;
using System.Data.Common;
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
                if (!_dbContext.Auctions.Any()) return;

                var auctions = _dbContext.Auctions.Select(auction => new AuctionHistory
                {
                    BuyerName = auction.BuyerName,
                    ItemForSale = auction.ItemSoldId,
                    SellerName = auction.SellerName,
                    SoldFor = auction.SoldFor,
                    SoldOn = auction.SoldOn
                }).ToList();

                History = auctions.ToList();
                _logManager.Boot("Loaded {0} Auctions", History.Count());
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }

        public void Save()
        {
            try
            {
                foreach (var history in History.Where(x => !x.Saved).ToList())
                {
                    var auction = new DAL.Models.Auction
                    {
                        BuyerName = history.BuyerName,
                        ItemSoldId = history.ItemForSale,
                        SellerName = history.SellerName,
                        SoldFor = history.SoldFor,
                        SoldOn = history.SoldOn
                    };
                    _dbContext.Auctions.Attach(auction);
                    history.Saved = true;
                }
                _dbContext.SaveChanges();
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }
    }
}
