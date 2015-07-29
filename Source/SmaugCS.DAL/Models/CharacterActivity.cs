using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterActivities")]
    public class CharacterActivity : Entity
    {
        public int PvPKills { get; set; }

        public int PvPDeaths { get; set; }

        public int PvPTimer { get; set; }

        public int PvEKills { get; set; }

        public int PvEDeaths { get; set; }

        public int IllegalPvP { get; set; }

        public long PlayedTime { get; set; }

        public long IdleTime { get; set; }

        public int AuctionBidsPlaced { get; set; }

        public int AuctionsWon { get; set; }

        public int AuctionsStarted { get; set; }

        public long CoinEarned { get; set; }
    }
}
