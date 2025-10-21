using System;
using System.Collections.Generic;

namespace Patterns.Factory;

public abstract class Factory<TK, TV> : IFactory<TK, TV>
  where TK : Type
  where TV : class, new()
{
  private readonly SortedList<TK, CreateFunctor> _products = [];

  private delegate TV CreateFunctor();

  public virtual void Register<TU>(TK key) where TU : TV, new()
  {
    CreateFunctor createFunctor = Creator<TU>;
    _products.Add(key, createFunctor);
  }

  public virtual TV Create(TK key) => !_products.TryGetValue(key, out CreateFunctor value) ? null : value();

  public virtual TV Creator<TU>() where TU : TV, new() => new TU();
}