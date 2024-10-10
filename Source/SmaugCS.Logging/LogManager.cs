using Autofac.Features.AttributeFilters;
using Library.Common;
using Library.Common.Logging;
using Library.Lua;
using SmaugCS.Common.Enumerations;
using SmaugCS.DAL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Timers;
using SmaugCS.DAL.Models;

namespace SmaugCS.Logging;

public sealed class LogManager : ILogManager
{
  public ILogWrapper LogWrapper { get; private set; }

  private readonly IDbContext _dbContext;
  private readonly List<LogEntry> _pendingLogs;
  private readonly ITimer _dbDumpTimer;
  private readonly int _sessionId;

  public LogManager(ILogWrapper logWrapper, [KeyFilter("LogDumpTimer")] ITimer timer, IDbContext dbContext)
  {
    LogWrapper = logWrapper;
    _dbContext = dbContext;

    _pendingLogs = [];

    _dbDumpTimer = timer;
    _dbDumpTimer.Elapsed += DbDumpTimerOnElapsed;

    if (_dbDumpTimer.Interval <= 0)
      _dbDumpTimer.Interval = 500;

    _dbDumpTimer.Start();
  }

  ~LogManager()
  {
    if (_dbDumpTimer == null) return;
    _dbDumpTimer.Stop();
    _dbDumpTimer.Dispose();
  }

  private void DbDumpTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
  {
    if (_pendingLogs.Count == 0) return;

    List<LogEntry> logsToDump = [.._pendingLogs];
    _pendingLogs.Clear();

    try
    {
      foreach (Log logToSave in logsToDump.Select(log => new Log { LogType = log.LogType, Text = log.Text, SessionId = _sessionId }))
      {
        _dbContext.AddOrUpdateAsync(logToSave);
      }
    }
    catch (DbException ex)
    {
      DatabaseFailureLog("{0}\n{1}", ex.Message, ex.StackTrace);

      if (logsToDump.Count != 0)
      {
        _pendingLogs.AddRange(logsToDump);
        logsToDump.Clear();
      }
    }
  }

  public void DatabaseFailureLog(string str, params object[] args)
  {
    StringBuilder sb = new();
    sb.AppendFormat(str, args);
    LogWrapper.Info($"[FATAL] {sb}");
  }

  #region Boot Log

  public void Boot(string str, params object[] args)
  {
    StringBuilder sb = new();
    sb.AppendFormat(str, args);
    LogWrapper.Info($"[BOOT] {sb}");
  }

  public void Boot(Exception ex)
  {
    Boot($"{ex.Message}\n{ex.StackTrace}");
  }

  #endregion

  private void Log(LogTypes logType, string str, params object[] args)
  {
    LogEntry entry = new()
    {
      LogType = logType,
      Text = string.Format(str, args)
    };
    _pendingLogs.Add(entry);
  }

  #region Bug Log

  public void Bug(string str, params object[] args)
  {
    Log(LogTypes.Bug, str, args);
  }

  public void Bug(Exception ex)
  {
    Bug(ex.Message + "\n{0}", ex.StackTrace);
  }

  #endregion

  #region Error Log

  public void Error(Exception ex)
  {
    Error(ex.Message + "\n{0}", ex.StackTrace);
  }

  public void Error(string str, params object[] args)
  {
    Log(LogTypes.Error, str, args);
  }

  #endregion

  #region Info Log

  public void Info(string str, params object[] args)
  {
    Log(LogTypes.Info, str, args);
  }

  [LuaFunction("LLog", "Logs a string", "Text to log")]
  public void LuaLog(string txt)
  {
    Info(txt);
  }

  #endregion
}