using System;

namespace Library.Common.Logging;

public class LogEventArgs : EventArgs
{
    public LogLevel Level { get; set; }

    public string Name { get; set; }

    public string Text { get; set; }
}