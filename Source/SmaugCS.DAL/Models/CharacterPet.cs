using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterPets")]
    public class CharacterPet : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public int MonsterId { get; set; }

        public int RoomId { get; set; }

        public string OverrideName { get; set; }

        public string OverrideShortDescription { get; set; }

        public string OverrideLongDescription { get; set; }

        public int Position { get; set; }

        public int Flags { get; set; }
    }
}
