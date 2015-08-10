using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmaugCS.DAL.Models
{
    [Table("NewsEntries")]
    public class NewsEntry : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime PostedOn { get; set; }

        public string PostedBy { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }
}
