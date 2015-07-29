using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("CharacterLanguages")]
    public class CharacterLanguage : Entity
    {
        public string LanguageName { get; set; }
    }
}
