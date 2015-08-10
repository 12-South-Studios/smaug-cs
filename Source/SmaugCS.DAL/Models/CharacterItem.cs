using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterItems")]
    public class CharacterItem : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public long ItemId { get; set; }

        public int Count { get; set; }

        public int Location { get; set; }

        public int Flags { get; set; }

        public long ContainedInId { get; set; }

        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public int Value3 { get; set; }

        public int Value4 { get; set; }

        public int Value5 { get; set; }

        public int Value6 { get; set; }

        public string OverrideName { get; set; }

        public string OverrideShortDescription { get; set; }

        public string OverrideLongDescription { get; set; }
    }
}
