using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("Bans")]
    public class Ban : Entity
    {
        public BanTypes BanType { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1024)]
        public string Note { get; set; }

        [MaxLength(25)]
        public string BannedBy { get; set; }

        public DateTime BannedOn { get; set; }

        public int Duration { get; set; }

        public int Level { get; set; }

        public bool IsWarning { get; set; }

        public bool IsPrefix { get; set; }

        public bool IsSuffix { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
