using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterSkills")]
    public class CharacterSkill : Entity
    {
        [Required]
        public int CharacterId { get; set; }

        [ForeignKey("CharacterId")]
        public virtual Character Character { get; set; }

        public SkillTypes SkillType { get; set; }

        public string SkillName { get; set; }

        public int LearnedValue { get; set; }
    }
}
