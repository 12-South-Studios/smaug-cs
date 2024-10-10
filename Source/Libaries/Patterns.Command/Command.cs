using System.Collections.Generic;

namespace Patterns.Command;

public abstract class Command(string name) : ICommand
{
  public string Name { get; } = name;

  public abstract void Execute(IDictionary<string, object> args = null);

  public virtual bool CanExecute(object obj) => true;
}