using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("Logs")]
    public class Log : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public LogTypes LogType { get; set; }

        public string Text { get; set; }

        public int SessionId { get; set; }
        
        [ForeignKey("SessionId")]
        public virtual Session Session { get; set; }
    }
}
