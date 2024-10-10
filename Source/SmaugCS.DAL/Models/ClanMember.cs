using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("ClanMembers")]
public class ClanMember : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public int ClanId { get; set; }
    public int ClanRank { get; set; }

    public string Name { get; set; }
    public DateTime Joined { get; set; }
    public int ClassId { get; set; }
    public int Level { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public string Notes { get; set; }
}