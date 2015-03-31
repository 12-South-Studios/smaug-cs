using System;
using System.Linq;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using SmaugCS.Constants;
using SmaugCS.Data;
using SmaugCS.Data.Instances;

namespace SmaugCS.Auction
{
    public sealed class AuctionManager : IAuctionManager
    {
        private static IKernel _kernel;
        private static ITimer _timer;

        public AuctionData Auction { get; set; }

        public IAuctionRepository Repository { get; private set; }

        public AuctionManager(IKernel kernel, ITimer timer, IAuctionRepository repository)
        {
            _kernel = kernel;
            Repository = repository;

            _timer = timer;

            if (_timer.Interval <= 0)
                _timer.Interval = 30000;

            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        ~AuctionManager()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }

        public static IAuctionManager Instance
        {
            get { return _kernel.Get<IAuctionManager>(); }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            Repository.Load();
        }

        public void Save()
        {
            Repository.Save();
        }

        public void StartAuction(CharacterInstance seller, ObjectInstance item, int startingPrice)
        {
            if (Auction != null)
                throw new AuctionAlreadyStartedException(
                    "New auction by Character {0} for Item {1} cannot be started as an auction is already in progress.",
                    seller.ID, item.ID);

            AuctionData auction = new AuctionData
            {
                ItemForSale = item,
                StartingBid = startingPrice,
                Buyer = seller,
                Seller = seller,
                PulseFrequency = GameConstants.GetSystemValue<int>("PulseAuction")
            };

            Auction = auction;
            Repository.Add(auction);
        }

        public void PlaceBid(CharacterInstance bidder, int bidAmount)
        {
            if (Auction == null)
                throw new NoAuctionStartedException("Bidder {0} attempted to bid on an auction that doesn't exist",
                    bidder.ID);
            if (Auction.BidAmount > bidAmount)
                throw new InvalidBidException(
                    "Bidder {0} attempted to place a bid of {1} on Auction {2} (Current Bid {3})",
                    bidder.ID, bidAmount, Auction.ItemForSale.ID, Auction.BidAmount);

            Auction.Buyer = bidder;
            Auction.BidAmount = bidAmount;
            Auction.GoingCounter = 0;
            Auction.PulseFrequency = GameConstants.GetSystemValue<int>("PulseAuction");
        }

        public void StopAuction()
        {
            if (Auction == null)
                throw new NoAuctionStartedException("An attempt was made to stop an auction, but one doesn't exist");

            AuctionHistory history =
                Repository.History.ToList()
                    .FirstOrDefault(x => x.ItemForSale == Auction.ItemForSale.ID && x.SellerName == Auction.Seller.Name);
            if (history != null)
                Repository.History.ToList().Remove(history);

            Auction = null;
        }
    }
}
