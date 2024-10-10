using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("Characters")]
public class Character : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Description { get; set; }

    public int Gender { get; set; }

    public int School { get; set; }

    public int Race { get; set; }

    public DateTime DeletedOnUtc { get; set; }

    public int Age { get; set; }

    public int Level { get; set; }

    public int RoomId { get; set; }

    public string AuthedBy { get; set; }

    public long CouncilId { get; set; }

    public int DeityId { get; set; }

    public long ClanId { get; set; }

    public virtual ICollection<CharacterActivity> ActivityHistory { get; set; }

    public virtual ICollection<CharacterAffect> Affects { get; set; }

    public virtual ICollection<CharacterFriend> Friends { get; set; }

    public virtual ICollection<CharacterIgnore> Ignored { get; set; }

    public virtual CharacterImmortal ImmortalData { get; set; }

    public virtual ICollection<CharacterItem> Items { get; set; }

    public virtual ICollection<CharacterLanguage> Languages { get; set; }

    public virtual ICollection<CharacterLogin> LoginHistory { get; set; }

    public virtual CharacterPet Pet { get; set; }

    public virtual ICollection<CharacterPvEHistory> PvEHistory { get; set; }

    public virtual ICollection<CharacterSkill> Skills { get; set; }

    public virtual ICollection<CharacterStatistic> Statistics { get; set; }
}