using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterImmortals")]
    public class CharacterImmortal : Entity
    {
        public string BamfinMessage { get; set; }

        public string BamfoutMessage { get; set; }

        public int Trust { get; set; }

        public int WizInvis { get; set; }

        public string ImmortalRank { get; set; }
    }
}
