using LuaInterface;
using System.Linq;
using System.Reflection;

namespace Library.Lua;

/// <summary>
/// 
/// </summary>
public class LuaInterfaceProxy
{
  private readonly LuaInterface.Lua _lua;

  /// <summary>
  /// 
  /// </summary>
  public LuaInterfaceProxy()
  {
    _lua = new LuaInterface.Lua();
    if (_lua == null)
      throw new LuaException("Unable to initialize the Lua Virtual Machine");
  }

  /// <summary>
  /// 
  /// </summary>
  public void Close() => _lua.Close();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public LuaTable CreateTable(string name)
  {
    _lua.NewTable(name);
    return _lua.GetTable(name);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="fileName"></param>
  /// <returns></returns>
  public object[] DoFile(string fileName) => _lua.DoFile(fileName);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="fileName"></param>
  /// <returns></returns>
  public object[] DoFileWithLoad(string fileName)
  {
    LuaFunction luaFunc = _lua.LoadFile(fileName);
    return luaFunc.Call(null);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="chunk"></param>
  /// <returns></returns>
  public object[] DoString(string chunk) => _lua.DoString(chunk);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="chunk"></param>
  /// <param name="func"></param>
  /// <returns></returns>
  public object[] DoStringWithLoad(string chunk, string func)
  {
    LuaFunction luaFunc = _lua.LoadString(chunk, func);
    return luaFunc.Call(null);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="fullPath"></param>
  /// <returns></returns>
  public LuaFunction GetFunction(string fullPath) => _lua.GetFunction(fullPath);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="repository"></param>
  public void RegisterFunctions(LuaFunctionRepository repository)
  {
    repository.Values.ToList().ForEach(x => RegisterFunction(x.Name, null, x.Info));
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="path"></param>
  /// <param name="target"></param>
  /// <param name="function"></param>
  /// <returns></returns>
  public LuaFunction RegisterFunction(string path, object target, MethodBase function)
  {
    return _lua.RegisterFunction(path, target, function);
  }
}