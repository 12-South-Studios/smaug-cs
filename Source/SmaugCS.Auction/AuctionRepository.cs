using SmaugCS.DAL;
using SmaugCS.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace SmaugCS.Auction
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuctionRepository : IAuctionRepository
    {
        public IEnumerable<AuctionHistory> History { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IDbContext _dbContext;

        public AuctionRepository(ILogManager logManager, IDbContext dbContext)
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
                if (_dbContext.Count<DAL.Models.Auction>() == 0) return;

                var auctions = _dbContext.GetAll<DAL.Models.Auction>().Select(auction => new AuctionHistory
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
                    _dbContext.AddOrUpdate<DAL.Models.Auction>(auction);
                    history.Saved = true;
                }
            }
            catch (DbException ex)
            {
                _logManager.Error(ex);
                throw;
            }
        }
    }
}
