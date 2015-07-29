using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using SmaugCS.Common.Enumerations;
using SmaugCS.DAL.Interfaces;

namespace SmaugCS.Logging
{
    public sealed class LogManager : ILogManager
    {
        public ILogWrapper LogWrapper { get; private set; }

        private static IKernel _kernel;
        private readonly ISmaugDbContext _dbContext;
        private readonly List<LogEntry> _pendingLogs;
        private readonly ITimer _dbDumpTimer;

        public static ILogManager Instance { get { return _kernel.Get<ILogManager>(); } }

        public LogManager(ILogWrapper logWrapper, IKernel kernel, ITimer timer, ISmaugDbContext dbContext)
        {
            LogWrapper = logWrapper;
            _kernel = kernel;
            _dbContext = dbContext;

            _pendingLogs = new List<LogEntry>();

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
            if (!_pendingLogs.Any()) return;

            var logsToDump = new List<LogEntry>(_pendingLogs);
            _pendingLogs.Clear();

            using (var transaction = _dbContext.ObjectContext.Connection.BeginTransaction())
            {
                try
                {
                    foreach (var log in logsToDump)
                    {
                        var logToSave = new DAL.Models.Log
                        {
                            LogType = log.LogType,
                            LoggedOn = DateTime.UtcNow,
                            Text = log.Text
                        };
                        _dbContext.Logs.Attach(logToSave);
                    }
                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (DbException ex)
                {
                    transaction.Rollback();
                    DatabaseFailureLog("{0}\n{1}", ex.Message, ex.StackTrace);

                    if (logsToDump.Any())
                    {
                        _pendingLogs.AddRange(logsToDump);
                        logsToDump.Clear();
                    }
                }
            }
        }

        public void DatabaseFailureLog(string str, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(str, args);
            LogWrapper.InfoFormat("[FATAL] {0}", sb.ToString());
        }

        #region Boot Log
        public void Boot(string str, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(str, args);
            LogWrapper.InfoFormat("[BOOT] {0}", sb.ToString());
        }

        public void Boot(Exception ex)
        {
            Boot(ex.Message + "\n{0}", ex.StackTrace);
        }
        #endregion

        private void Log(LogTypes logType, string str, params object[] args)
        {
            var entry = new LogEntry
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
}
