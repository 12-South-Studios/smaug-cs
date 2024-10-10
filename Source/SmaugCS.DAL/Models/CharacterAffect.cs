using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("CharacterAffects")]
public class CharacterAffect : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public string AffectType { get; set; }

    public int Duration { get; set; }

    public int Modifier { get; set; }

    public int Location { get; set; }

    public int Flags { get; set; }
}