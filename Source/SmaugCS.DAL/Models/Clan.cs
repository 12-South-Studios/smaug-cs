using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("Clans")]
    public class Clan : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("BoardId")]
        public virtual Board Board { get;set; }
        public int BoardId { get; set; }

        public int ClanType { get; set; }
        public string Motto { get; set; }
        public string DeityName { get; set; }
        public string Badge { get; set; }
        
        [ForeignKey("ClanStatsId")]
        public virtual ClanStats Stats { get; set; }
        public int ClanStatsId { get; set; }

        public int RecallRoomId { get; set; }
        public int StoreRoomId { get; set; }
        // TODO GuardOne
        // TODO GuardTwo
        // TODO Class
        
        public virtual ICollection<ClanRank> Ranks { get; set; }
        public virtual ICollection<ClanMember> Members { get; set; }
        public virtual ICollection<ClanItem> Items { get; set; }
    }
}
