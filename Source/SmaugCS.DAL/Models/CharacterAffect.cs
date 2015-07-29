using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterAffects")]
    public class CharacterAffect : Entity
    {
        public string AffectType { get; set; }

        public int Duration { get; set; }

        public int Modifier { get; set; }

        public int Location { get; set; }

        public int Flags { get; set; }
    }
}
