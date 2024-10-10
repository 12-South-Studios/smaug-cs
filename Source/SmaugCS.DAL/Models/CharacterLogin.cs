using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("CharacterLogins")]
public class CharacterLogin : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public DateTime LoginDate { get; set; }

    public string IpAddress { get; set; }

    public int SessionId { get; set; }

    [ForeignKey("SessionId")]
    public virtual Session Session { get; set; }
}