using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterPets")]
    public class CharacterPet : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public int MonsterId { get; set; }

        public int RoomId { get; set; }

        public string OverrideName { get; set; }

        public string OverrideShortDescription { get; set; }

        public string OverrideLongDescription { get; set; }

        public int Position { get; set; }

        public int Flags { get; set; }

        public virtual ICollection<CharacterItem> Items { get; set; } 
    }
}
