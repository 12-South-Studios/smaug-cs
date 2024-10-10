using System;
using System.Collections;
using System.Reflection;

namespace Library.Lua;

/// <summary>
/// Defines a description for a Lua Function
/// </summary>
public sealed class LuaFunctionDescriptor
{
  /// <summary>
  /// Initializes a new instance of the LuaFunctionDescriptor class.
  /// </summary>
  /// <param name="name">Name of the function</param>
  /// <param name="description">Description of the function</param>
  /// <param name="parameters">Hashtable of parameter strings</param>
  /// <param name="info"></param>
  public LuaFunctionDescriptor(string name, string description, IDictionary parameters, MethodInfo info)
  {
    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
    if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

    Name = name;
    Description = description;
    Parameters = parameters;
    Info = info;

    string strFunctionHeader = $"{name}(%params%) - {description}";
    string strFunctionBody = "\n\n";
    string strFunctionParams = string.Empty;

    bool first = true;
    foreach (DictionaryEntry entry in parameters)
    {
      if (!first)
        strFunctionParams += ", ";

      strFunctionParams += entry.Key;
      strFunctionBody += $"\t{entry.Key}\t\t{entry.Value}\n";
      first = false;
    }

    strFunctionBody = strFunctionBody[..^1];
    if (first)
      strFunctionBody = strFunctionBody[..^1];

    FullDescription = strFunctionHeader.Replace("%params%", strFunctionParams) + strFunctionBody;
  }

  /// <summary>
  /// Gets the name of the function
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Gets the description of the function
  /// </summary>
  public string Description { get; }

  /// <summary>
  /// Gets the parameter list of the function
  /// </summary>
  public IDictionary Parameters { get; }

  /// <summary>
  ///
  /// </summary>
  public MethodInfo Info { get; }

  /// <summary>
  /// Gets the header of the function
  /// </summary>
  public string Header => !FullDescription.Contains('\n')
    ? FullDescription
    : FullDescription[..FullDescription.IndexOf('\n')];

  /// <summary>
  /// Gets the full description
  /// </summary>
  public string FullDescription { get; }
}