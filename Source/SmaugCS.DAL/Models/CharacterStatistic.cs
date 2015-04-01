using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterStatistics")]
    public class CharacterStatistic : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public StatisticTypes Statistic { get; set; }

        public int? IntValue { get; set; }

        public string StringValue { get; set; }
    }
}
