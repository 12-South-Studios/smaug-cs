using SmaugCS.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterStatistics")]
    public class CharacterStatistic : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public StatisticTypes Statistic { get; set; }

        public int? IntValue { get; set; }

        public string StringValue { get; set; }
    }
}
