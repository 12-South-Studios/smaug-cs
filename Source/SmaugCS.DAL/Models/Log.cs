using System;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("Logs")]
    public class Log : Entity
    {
        public LogTypes LogType { get; set; }

        public DateTime LoggedOn { get; set; }

        public string Text { get; set; }
    }
}
