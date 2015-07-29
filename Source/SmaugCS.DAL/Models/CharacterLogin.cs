using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterLogins")]
    public class CharacterLogin : Entity
    {
        public DateTime LoginDate { get; set; }

        public string IpAddress { get; set; }
    }
}
