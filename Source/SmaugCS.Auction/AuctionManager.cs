﻿using Autofac.Features.AttributeFilters;
using Library.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Data.Instances;
using System.Linq;
using System.Timers;

namespace SmaugCS.Auction;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class AuctionManager : IAuctionManager
{
  private static ITimer _timer;

  public AuctionData Auction { get; private set; }

  public IAuctionRepository Repository { get; }

  public AuctionManager([KeyFilter("AuctionPulseTimer")] ITimer timer, IAuctionRepository repository)
  {
    Repository = repository;

    _timer = timer;

    if (_timer.Interval <= 0)
      _timer.Interval = 30000;

    _timer.Elapsed += TimerOnElapsed;
    _timer.Start();
  }

  ~AuctionManager()
  {
    if (_timer == null) return;
    _timer.Stop();
    _timer.Dispose();
  }

  private void TimerOnElapsed(object sender, ElapsedEventArgs e)
  {
    if (Auction != null)
      Save();
  }

  public void Initialize() => Repository.Load();

  public void Save() => Repository.Save();

  public AuctionData StartAuction(CharacterInstance seller, ObjectInstance item, int startingPrice)
  {
    if (Auction != null)
      throw new AuctionAlreadyStartedException(
        "New auction by Character {0} for Item {1} cannot be started as an auction is already in progress.",
        seller.Id, item.Id);

    AuctionData auction = new()
    {
      ItemForSale = item,
      StartingBid = startingPrice,
      Buyer = seller,
      Seller = seller,
      PulseFrequency = GameConstants.GetSystemValue<int>("PulseAuction")
    };

    Auction = auction;
    Repository.Add(auction);
    return auction;
  }

  public void PlaceBid(CharacterInstance bidder, int bidAmount)
  {
    if (Auction == null)
      throw new NoAuctionStartedException("Bidder {0} attempted to bid on an auction that doesn't exist",
        bidder.Id);
    if (Auction.BidAmount > bidAmount)
      throw new InvalidBidException(
        "Bidder {0} attempted to place a bid of {1} on Auction {2} (Current Bid {3})",
        bidder.Id, bidAmount, Auction.ItemForSale.Id, Auction.BidAmount);

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
        .FirstOrDefault(x => x.ItemForSale == Auction.ItemForSale.Id && x.SellerName == Auction.Seller.Name);
    if (history != null)
      Repository.History.ToList().Remove(history);

    Auction = null;
  }
}