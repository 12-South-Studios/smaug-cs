using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("NewsEntries")]
    public class NewsEntry : Entity
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime PostedOn { get; set; }

        public string PostedBy { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
