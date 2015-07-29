using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterIgnored")]
    public class CharacterIgnore : Entity
    {
        public string IgnoredName { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
