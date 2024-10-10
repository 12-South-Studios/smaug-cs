using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("GameStates")]
public class GameState : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public int GameYear { get; set; }

    public int GameMonth { get; set; }

    public int GameDay { get; set; }

    public int GameHour { get; set; }
}