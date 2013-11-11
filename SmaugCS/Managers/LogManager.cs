using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using log4net;
using Realm.Library.Common;
using Realm.Library.Common.Objects;

namespace SmaugCS.Managers
{
    public sealed class LogManager : GameSingleton
    {
        private static LogManager _instance;
        private static readonly object Padlock = new object();

        private static readonly ILog Logger = log4net.LogManager.GetLogger("SMAUG");

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

        public static void Bug(string str, params object[] args)
        {
            StringBuilder sb = new StringBuilder();

            var method = new StackTrace().GetFrame(2).GetMethod();
            if (method != null && method.DeclaringType != null)
                sb.AppendFormat("{0}:{1} ", method.DeclaringType.FullName, method.Name);

            sb.AppendFormat(str, args);
            Logger.Error(sb.ToString());
        }

        public static void BootLog(string str, params object[] args)
        {
            string path = SystemConstants.GetSystemFile(SystemFileTypes.Bootlog);
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(str, args);
                proxy.Write("[*****] BOOT: {0}\n", sb.ToString());
            }
        }

        public static void Log(LogTypes logType, int level, string fmt, params object[] args)
        {
            Log(string.Format(fmt, args), logType, level);
        }

        public static void Log(string fmt, params object[] args)
        {
            Log(string.Format(fmt, args), LogTypes.Normal, Program.LEVEL_LOG);
        }

        public static void Log(string str, LogTypes logType, int level)
        {
            string buffer = string.Format("{0} :: {1}\n", DateTime.Now, str);
            switch (logType)
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
            }
        }

        public static void Log(string txt)
        {
            Log(txt, (short)LogTypes.Normal, (short)Program.LEVEL_LOG);
        }
    }
}
