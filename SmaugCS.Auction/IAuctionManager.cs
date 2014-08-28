namespace SmaugCS.Auction
{
    public interface IAuctionManager
    {
        void Initialize();
        void Save();

        AuctionData Auction { get; set; }
    }
}
