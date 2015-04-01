using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterAffects")]
    public class CharacterAffect : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public string AffectType { get; set; }

        public int Duration { get; set; }

        public int Modifier { get; set; }

        public int Location { get; set; }

        public int Flags { get; set; }
    }
}
