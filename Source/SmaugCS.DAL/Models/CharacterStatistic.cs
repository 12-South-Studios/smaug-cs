using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterStatistics")]
    public class CharacterStatistic : Entity
    {
        public StatisticTypes Statistic { get; set; }

        public int? IntValue { get; set; }

        public string StringValue { get; set; }
    }
}
