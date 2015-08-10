using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterSkills")]
    public class CharacterSkill : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public SkillTypes SkillType { get; set; }

        public string SkillName { get; set; }

        public int LearnedValue { get; set; }
    }
}
