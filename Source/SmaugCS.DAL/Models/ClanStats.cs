using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models;

[Table("ClanStats")]
public class ClanStats : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual int Id { get; set; }

    public DateTime? CreateDateUtc { get; set; }

    public int ClanId { get; set; }

    // TODO PvPKillTable
    // TODO PvPDeathTable
    public int PvE_Kills { get; set; }
    public int PvE_Deaths { get; set; }
    public int Illegal_PvP_Kills { get; set; }
    public int Score { get; set; }
    public int Favour { get; set; }
    public int Strikes { get; set; }
    public int MemberLimit { get; set; }
    public int Alignment { get; set; }

}