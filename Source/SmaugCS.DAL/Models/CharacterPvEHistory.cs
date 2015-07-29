using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterPvEHistories")]
    public class CharacterPvEHistory : Entity
    {
        public int MonsterId { get; set; }

        public int TimesKilled { get; set; }
    }
}
