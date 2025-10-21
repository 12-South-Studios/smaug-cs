using Library.Common.Extensions;
using System;
using System.Collections.Generic;
using Library.Common.Entities;
using Library.Common.Objects;

namespace Library.Common.Contexts;

/// <summary>
///
/// </summary>
public class PropertyContext : BaseContext<IEntity>, IPropertyContext
{
  private readonly Dictionary<string, Property> _properties = [];

  /// <summary>
  ///
  /// </summary>
  /// <param name="owner"></param>
  public PropertyContext(IEntity owner)
    : base(owner)
  {
  }

  /// <summary>
  ///
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="name"></param>
  /// <returns></returns>
  public T GetProperty<T>(string name) => _properties.TryGetValue(name, out Property value) ? (T)value.Value : default;

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <param name="value"></param>
  /// <param name="bits"></param>
  public void SetProperty(string name, object value, PropertyTypeOptions bits = 0)
  {
    if (_properties.TryGetValue(name, out Property property))
    {
      if (property.Volatile)
        property.Value = value;
    }
    else
    {
      _properties.Add(name, new Property(name)
      {
        Value = value,
        Persistable = (bits & PropertyTypeOptions.Persistable) != 0,
        Volatile = (bits & PropertyTypeOptions.Volatile) != 0,
        Visible = (bits & PropertyTypeOptions.Visible) != 0
      });
    }
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="aEnum"></param>
  /// <param name="aValue"></param>
  /// <param name="bits"></param>
  public void SetProperty(Enum aEnum, object aValue, PropertyTypeOptions bits = 0)
  {
    SetProperty(aEnum.GetName(), aValue, bits);
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool HasProperty(string name) => _properties.ContainsKey(name);

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsPersistable(string name)
  {
    if (!_properties.TryGetValue(name, out Property property)) return false;
    return property.IsNotNull() && property.Persistable;
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsVolatile(string name)
  {
    if (!_properties.TryGetValue(name, out Property property)) return false;
    return property.IsNotNull() && property.Volatile;
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsVisible(string name)
  {
    if (!_properties.TryGetValue(name, out Property property)) return false;
    return property.IsNotNull() && property.Visible;
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool RemoveProperty(string name) => _properties.ContainsKey(name) && _properties.Remove(name);

  /// <summary>
  ///
  /// </summary>
  public IEnumerable<string> PropertyKeys => _properties.Keys;

  /// <summary>
  ///
  /// </summary>
  public int Count => _properties.Count;

  /// <summary>
  ///
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public string GetPropertyBits(string name)
  {
    string returnVal = string.Empty;

    if (!_properties.TryGetValue(name, out Property property)) return returnVal;

    string bits = string.Empty;
    if (property.Persistable) bits += "p";
    if (property.Volatile) bits += "v";
    if (property.Visible) bits += "i";

    returnVal = bits;
    return returnVal;
  }
}