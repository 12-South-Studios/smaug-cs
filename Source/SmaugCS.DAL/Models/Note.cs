using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("Notes")]
    public class Note : Entity
    {
        public string Sender { get; set; }

        public DateTime DateSent { get; set; }

        public string RecipientList { get; set; }

        public string Subject { get; set; }

        public bool IsPoll { get; set; }

        public string Text { get; set; }
    }
}
