using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("News")]
    public class News : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public string Name { get; set; }

        public string Header { get; set; }

        public int Level { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<NewsEntry> Entries { get; set; }
    }
}
