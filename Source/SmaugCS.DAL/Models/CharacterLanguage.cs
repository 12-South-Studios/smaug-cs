using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("CharacterLanguages")]
public class CharacterLanguage : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public string LanguageName { get; set; }
}