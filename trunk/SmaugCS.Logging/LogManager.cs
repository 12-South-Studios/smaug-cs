using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Realm.Library.Common;
using Realm.Library.Common.Logging;
using Realm.Library.Common.Objects;
using Realm.Library.Lua;
using log4net;

namespace SmaugCS.Logging
{
    public sealed class LogManager : GameSingleton, ILogManager
    {
        private static LogManager _instance;
        private static readonly object Padlock = new object();

        private static string _dataPath;
        public ILogWrapper LogWrapper { get; private set; }

        private static string BootLogFormat = "[BOOT] {0}";
        private static string BugLogFormat = "[BUG] {0}";
        private static string LogFormat = "{0}";

        private LogManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public static LogManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new LogManager());
                }
            }
        }

        public void Initialize(ILogWrapper logWrapper, string path)
        {
            LogWrapper = logWrapper;
            _dataPath = path;
        }

        #region Boot Log
        [LuaFunction("LBootLog", "Logs an entry to the boot log", "Text to log")]
        public void LuaBootLog(string txt)
        {
            Boot(txt);
        }

        public void Boot(string str, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(str, args);
            LogWrapper.InfoFormat(BootLogFormat, sb.ToString());
        }

        public void Boot(Exception ex)
        {
            Boot(ex.Message + "\n{0}", ex.StackTrace);
        }
        #endregion

        #region Bug Log
        public void Bug(string str, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(str, args);
            LogWrapper.InfoFormat(BugLogFormat, sb.ToString());
        }
        #endregion

        #region Error Log
        public void Error(Exception ex)
        {
            LogWrapper.Error(ex.Message, ex);
        }

        public void Error(string str, params object[] args)
        {
            LogWrapper.ErrorFormat(str, args);
        }
        #endregion

        #region General Log
        public void Log(LogTypes logType, int level, string fmt, params object[] args)
        {
            Log(string.Format(fmt, args), logType, level);
        }

        public void Log(string fmt, params object[] args)
        {
            //Log(string.Format(fmt, args), LogTypes.Normal, LevelConstants.LEVEL_LOG);
        }

        public void Log(string str, LogTypes logType, int level)
        {
            LogWrapper.InfoFormat(LogFormat, str);
            //string buffer = string.Format("{0} :: {1}\n", DateTime.Now, str);
            /*switch (logType)
            {
                case LogTypes.Build:
                    ChatManager.to_channel(buffer, ChannelTypes.Build, "Build", level);
                    break;
                case LogTypes.Comm:
                    ChatManager.to_channel(buffer, ChannelTypes.Comm, "Comm", level);
                    break;
                case LogTypes.Warn:
                    ChatManager.to_channel(buffer, ChannelTypes.Warn, "Warn", level);
                    break;
                case LogTypes.All:
                    break;
                default:
                    ChatManager.to_channel(buffer, ChannelTypes.Log, "Log", level);
                    break;
            }*/
        }

        [LuaFunction("LLog", "Logs a string", "Text to log")]
        public void LuaLog(string txt)
        {
            //Log(txt, (short)LogTypes.Normal, (short)LevelConstants.LEVEL_LOG);
        }
        #endregion

    }
}
