namespace Patterns.Decorator;

/// <summary>
/// 
/// </summary>
public abstract class Decorator(IComponent component) : IComponent
{
  /// <summary>
  /// 
  /// </summary>
  public IComponent Component { get; } = component;
}