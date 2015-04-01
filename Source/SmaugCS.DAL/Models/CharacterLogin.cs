using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterLogins")]
    public class CharacterLogin : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public DateTime LoginDate { get; set; }

        public string IpAddress { get; set; }
    }
}
