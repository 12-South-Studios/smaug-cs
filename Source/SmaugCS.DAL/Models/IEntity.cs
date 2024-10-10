using System;

namespace SmaugCS.DAL.Models;

public interface IEntity
{
    int Id { get; set; }

    DateTime? CreateDateUtc { get; set; }
}