using System.Collections.Generic;

namespace Patterns.Command;

/// <summary>
/// 
/// </summary>
public interface ICommand
{
    /// <summary>
    /// 
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    void Execute(IDictionary<string, object> args = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    bool CanExecute(object obj);
}