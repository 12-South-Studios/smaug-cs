using System;
using SmaugCS.Common.Enumerations;
using SmaugCS.Logging;

namespace SmaugCS.Extensions;

public static class LogManagerExtensions
{
  public static void Log(this LogManager logManager, string str, LogTypes logType, int level)
  {
    string buffer = $"{DateTime.Now} :: {str}\n";
    logManager.Info(str);

    /*switch (logType)



         ase LogTypes.Build:

             hatManager.to_channel(buffer, ChannelTypes.Build, "Build", level);

             reak;

         ase LogTypes.Comm:

             hatManager.to_channel(buffer, ChannelTypes.Comm, "Comm", level);

             reak;

         ase LogTypes.Warn:

             hatManager.to_channel(buffer, ChannelTypes.Warn, "Warn", level);

             reak;

         ase LogTypes.All:

             reak;

         efault:

             hatManager.to_channel(buffer, ChannelTypes.Log, "Log", level);

             reak;

     */
  }
}