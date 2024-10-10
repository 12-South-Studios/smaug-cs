using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("CharacterImmortals")]
public class CharacterImmortal : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public string BamfinMessage { get; set; }

    public string BamfoutMessage { get; set; }

    public int Trust { get; set; }

    public int WizInvis { get; set; }

    public string ImmortalRank { get; set; }
}