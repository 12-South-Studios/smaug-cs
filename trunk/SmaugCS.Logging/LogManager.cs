using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Timers;
using Ninject;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Lua;
using SmallDBConnectivity;

namespace SmaugCS.Logging
{
    public sealed class LogManager : ILogManager
    {
        public ILogWrapper LogWrapper { get; private set; }

        private static IKernel _kernel;
        private ISmallDb SmallDb { get; set; }
        private IDbConnection Connection { get; set; }
        private readonly List<LogEntry> _pendingLogs;
        private readonly ITimer _dbDumpTimer;

        public static ILogManager Instance { get { return _kernel.Get<ILogManager>(); } }

        public LogManager(ILogWrapper logWrapper, IKernel kernel, ISmallDb smallDb, IDbConnection connection,
            ITimer timer)
        {
            LogWrapper = logWrapper;
            _kernel = kernel;
            SmallDb = smallDb;
            Connection = connection;

            _pendingLogs = new List<LogEntry>();

            _dbDumpTimer = timer;
            _dbDumpTimer.Elapsed += DbDumpTimerOnElapsed;

            if (_dbDumpTimer.Interval <= 0)
                _dbDumpTimer.Interval = 500;

            _dbDumpTimer.Start();
        }

        ~LogManager()
        {
            if (_dbDumpTimer != null)
            {
                _dbDumpTimer.Stop();
                _dbDumpTimer.Dispose();
            }
        }

        private void DbDumpTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!_pendingLogs.Any()) return;

            List<LogEntry> logsToDump = new List<LogEntry>(_pendingLogs);
            _pendingLogs.Clear();

            IDbTransaction scope = Connection.BeginTransaction();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@tvpLogTable", SqlDbType.Structured)
                    {
                        Value = LogEntry.GetLogEntryDataTable(logsToDump)
                    }
                };
                SmallDb.ExecuteNonQuery(Connection, "cp_AddLog", parameters);
                scope.Commit();
            }
            catch (DbException ex)
            {
                DatabaseFailureLog("{0}\n{1}", ex.Message, ex.StackTrace);

                if (scope != null)
                    scope.Rollback();

                if (logsToDump.Any())
                {
                    _pendingLogs.AddRange(logsToDump);
                    logsToDump.Clear();
                }
            }
            finally
            {
                if (scope != null)
                    scope.Dispose();
                logsToDump.Clear();
            }
        }

        public void DatabaseFailureLog(string str, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(str, args);
            LogWrapper.InfoFormat("[FATAL] {0}", sb.ToString());
        }

        #region Boot Log
        public void Boot(string str, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
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
            LogEntry entry = new LogEntry
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
