using SmaugCS.Common.Enumerations;
using SmaugCS.Logging;
using System;

// ReSharper disable CheckNamespace
namespace SmaugCS
// ReSharper restore CheckNamespace
{
    public static class LogManagerExtensions
    {
        public static void Log(this LogManager logManager, string str, LogTypes logType, int level)
        {
            var buffer = $"{DateTime.Now} :: {str}\n";
            logManager.Info(str);

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
    }
}
