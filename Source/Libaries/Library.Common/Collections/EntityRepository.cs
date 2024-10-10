using Library.Common.Entities;
using Patterns.Repository;

namespace Library.Common.Collections;

/// <summary>
/// Stores objects that implement IEntity, derives from Repository
/// </summary>
public class EntityRepository : Repository<long, IEntity>, IEntityRepository;