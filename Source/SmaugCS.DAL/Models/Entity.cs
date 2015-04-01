using System;
using System.ComponentModel.DataAnnotations;

namespace SmaugCS.DAL.Models
{
    public abstract class Entity : IEntity
    {
        [Key]
        public virtual int Id { get; set; }

        public DateTime? CreateDateUtc { get; set; }
    }
}
