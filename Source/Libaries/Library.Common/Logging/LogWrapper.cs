using Microsoft.Extensions.Logging;
using System;

namespace Library.Common.Logging;

public class LogWrapper(ILogger<ILogWrapper> logger) : ILogWrapper
{
  public ILogger Logger { get; } = logger;
  public event EventHandler OnLog;

  private void LogToEventHandler(LogLevel level, string message, params object[] args)
  {
    if (OnLog == null) return;

    LogEventArgs e = new LogEventArgs
    {
      Level = level,
      Text = string.Format(message, args)
    };
    OnLog.Invoke(this, e);
  }

  public void Info(string message, params object[] args)
  {
    Logger.LogInformation(message, args);
    LogToEventHandler(LogLevel.Info, message, args);
  }

  public void Error(Exception ex)
  {
    Logger.LogError(ex.Message);
    LogToEventHandler(LogLevel.Error, ex.Message);
  }

  public void Error(string message, params object[] args)
  {
    Logger.LogError(message, args);
    LogToEventHandler(LogLevel.Error, message, args);
  }

  public void Debug(string message, params object[] args)
  {
    Logger.LogDebug(message, args);
    LogToEventHandler(LogLevel.Debug, message, args);
  }

  public void Warn(string message, params object[] args)
  {
    Logger.LogWarning(message, args);
    LogToEventHandler(LogLevel.Warn, message, args);
  }

  public void Fatal(string message, params object[] args)
  {
    Logger.LogCritical(message, args);
    LogToEventHandler(LogLevel.Fatal, message, args);
  }

  public void Log(int logType, string message, params object[] args) => Info(message, args);
}