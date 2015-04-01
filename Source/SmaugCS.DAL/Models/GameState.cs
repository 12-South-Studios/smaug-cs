using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("GameStates")]
    public class GameState : Entity
    {
        public int GameYear { get; set; }

        public int GameMonth { get; set; }

        public int GameDay { get; set; }

        public int GameHour { get; set; }
    }
}
