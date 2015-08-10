using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("Auctions")]
    public class Auction : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        [MaxLength(100)]
        public string SellerName { get; set; }

        [MaxLength(100)]
        public string BuyerName { get; set; }

        public DateTime SoldOn { get; set; }

        public int SoldFor { get; set; }

        public long ItemSoldId { get; set; }
    }
}
