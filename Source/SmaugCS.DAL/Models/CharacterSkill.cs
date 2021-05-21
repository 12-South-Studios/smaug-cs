using SmaugCS.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
