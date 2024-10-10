using Library.Common;
using Library.Common.Extensions;
using Library.Common.Logging;
using Library.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuaInterface;

namespace Library.Lua;

/// <inheritdoc />
///  <summary>
///  </summary>
public sealed class LuaVirtualMachine : Entity
{
  private readonly Dictionary<Task, CancellationTokenSource> _tasks;
  private readonly ILogWrapper _log;
  private readonly LuaFunctionRepository _repository;
  private readonly LuaInterfaceProxy _luaProxy;

  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  ///  <param name="id"></param>
  ///  <param name="log"></param>
  ///  <param name="repository"></param>
  /// <param name="luaProxy"></param>
  public LuaVirtualMachine(long id, ILogWrapper log, LuaFunctionRepository repository, LuaInterfaceProxy luaProxy)
    : base(id, "LuaVirtualMachine" + id)
  {
    ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

    _tasks = new Dictionary<Task, CancellationTokenSource>();
    _log = log;
    _repository = repository;
    _luaProxy = luaProxy;
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="script"></param>
  public void ExecuteScript(LuaScript script)
  {
    _log.Info("Script {0} submitted.", script.Name);

    Task<LuaResponse> task = null;
    CancellationTokenSource tokenSource = null;

    try
    {
      tokenSource = new CancellationTokenSource();
      task = Task.Factory.StartNew(() => ExecuteNextScript(tokenSource.Token, script), tokenSource.Token);

      _tasks.Add(task, tokenSource);
      task.Wait(tokenSource.Token);

      // Do something with result
    }
    catch (AggregateException ex)
    {
      _log.Info("Script {0} cancelled.", script.Name);
      ex.InnerException.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
    }
    catch (Exception ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
    }
    finally
    {
      if (task != null)
      {
        _tasks.Remove(task);
        task.Cancel(tokenSource);
      }
    }
  }

  /// <summary>
  /// Tells the virtual machine to re-register functions
  /// </summary>
  public void RegisterFunctions()
  {
    _repository.Values.ToList().ForEach(x => _luaProxy.RegisterFunction(x.Name, null, x.Info));
  }

  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  ///  <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      using Dictionary<Task, CancellationTokenSource>.Enumerator it = _tasks.GetEnumerator();
      while (it.MoveNext())
      {
        _tasks.Remove(it.Current.Key);
        it.Current.Key.Cancel(it.Current.Value);
      }
    }

    base.Dispose(disposing);
  }

  /// <summary>
  ///
  /// </summary>
  /// <param name="token"></param>
  /// <param name="script"></param>
  /// <returns></returns>
  private LuaResponse ExecuteNextScript(CancellationToken token, LuaScript script)
  {
    token.ThrowIfCancellationRequested();

    bool returnVal = false;
    List<object> responseList = [];

    try
    {
      object[] responseData = script.IsFile
        ? _luaProxy.DoFile(script.Script)
        : _luaProxy.DoString(script.Script);
      responseList = responseData.IsNotNull() ? responseData.ToList() : [];

      if (!string.IsNullOrEmpty(script.Function))
      {
        LuaFunction luaFunction = _luaProxy.GetFunction(script.Function);
        if (luaFunction == null)
          throw new LuaException(
            $"Function {script.Function} not found in Lua Script {script.Name}");

        responseData = luaFunction.Call(script.Table);
        if (responseData != null)
          responseList.AddRange(responseData.ToList());
      }

      returnVal = true;
    }
    catch (LuaException ex)
    {
      ex.Handle(ExceptionHandlingOptions.RecordAndThrow, _log);
    }

    return new LuaResponse
    {
      Success = returnVal,
      ResponseData = responseList.ToList()
    };
  }
}