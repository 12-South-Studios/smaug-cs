using Patterns.Repository;

namespace Library.Common.Entities;

/// <summary>
/// Public interface for a repository to store objects that implement IEntity
/// </summary>
public interface IEntityRepository : IRepository<long, IEntity>;