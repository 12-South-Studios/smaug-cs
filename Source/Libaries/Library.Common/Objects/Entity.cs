using System;
using System.Diagnostics.CodeAnalysis;
using Library.Common.Entities;

namespace Library.Common.Objects;

/// <summary>
/// Defines a generic disposable entity
/// </summary>
public abstract class Entity : Cell, IEntity, IDisposable
{
  /// <summary>
  /// Default constructor for the Entity class
  /// </summary>
  protected Entity(long id, string name)
  {
    Id = id;
    Name = name;
  }

  /// <summary>
  /// Destructor
  /// </summary>
  ~Entity()
  {
    Dispose(false);
  }

  /// <summary>
  /// Overrides the Equals function to perform a comparison of ID and Name
  /// </summary>
  public override bool Equals(object obj)
  {
    if (obj.IsNull() || GetType() != obj?.GetType())
      return false;

    Entity entity = obj.CastAs<Entity>();
    return entity.IsNotNull() && Id == entity.Id && Name.Equals(entity.Name, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <returns></returns>
  public static bool operator ==(Entity a, Entity b)
  {
    if (ReferenceEquals(a, b)) return true;
    if (a is null || b is null) return false;
    return a.Id == b.Id && a.Name.Equals(b.Name);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <returns></returns>
  public static bool operator !=(Entity a, Entity b)
  {
    return !(a == b);
  }

  #region IDisposable

  /// <summary>
  /// Overrides the base Dispose to make this object disposable
  /// </summary>
  [ExcludeFromCodeCoverage]
  public void Dispose()
  {
    Dispose(true);

    // Use SuppressFinalize in case a subclass
    // of this type implements a finalizer.
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Dispose of any internal resources
  /// </summary>
  /// <param name="disposing"></param>
  [ExcludeFromCodeCoverage]
  protected virtual void Dispose(bool disposing)
  {
    if (disposing)
    {
      // Nothing to dispose
    }
  }

  #endregion IDisposable
}