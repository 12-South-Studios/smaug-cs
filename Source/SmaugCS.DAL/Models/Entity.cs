using System;

namespace SmaugCS.DAL.Models;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }

    protected Entity()
    {
            CreatedUtc = ModifiedUtc = DateTime.UtcNow;
        }
}