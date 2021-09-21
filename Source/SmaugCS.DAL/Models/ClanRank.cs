using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmaugCS.DAL.Models
{
    [Table("ClanStats")]
    public class ClanRank : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public int ClanId { get; set; }

        public int RankType { get; set; }
        public string RankName { get; set; }
    }
}
