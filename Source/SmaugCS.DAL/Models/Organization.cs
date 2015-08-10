using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.DAL.Models
{
    [Table("Organizations")]
    public class Organization : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }

        public GroupTypes OrganizationType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Leader { get; set; }

        [Required]
        public int BoardId { get; set; }

        [ForeignKey("BoardId")]
        public virtual Board Board { get; set; }
    }
}
