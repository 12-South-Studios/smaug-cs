using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterSkills")]
    public class CharacterSkill : Entity
    {
        public SkillTypes SkillType { get; set; }

        public string SkillName { get; set; }

        public int LearnedValue { get; set; }
    }
}
