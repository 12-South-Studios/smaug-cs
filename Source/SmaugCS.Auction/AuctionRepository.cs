using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Infrastructure.Data;
using SmaugCS.Logging;

namespace SmaugCS.Auction
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuctionRepository : IAuctionRepository
    {
        public IEnumerable<AuctionHistory> History { get; private set; }
        private readonly ILogManager _logManager;
        private readonly IRepository _repository;

        public AuctionRepository(ILogManager logManager, IRepository repository)
        {
            History = new List<AuctionHistory>();
            _logManager = logManager;
            _repository = repository;
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
                foreach (var history in _repository.GetQuery<DAL.Models.Auction>().Select(auction => new AuctionHistory
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
                _repository.UnitOfWork.BeginTransaction();
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
                    _repository.Attach(auction);
                    history.Saved = true;
                }
                _repository.UnitOfWork.CommitTransaction();
            }
            catch (DbException ex)
            {
                _repository.UnitOfWork.RollBackTransaction();
                _logManager.Error(ex);
            }
        }
    }
}
