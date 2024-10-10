using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Library.Common;

public class TextWriterProxy(TextWriter tw) : IDisposable
{
  [ExcludeFromCodeCoverage]
  ~TextWriterProxy()
  {
    Dispose(false);
  }

  public virtual void Write(string value)
  {
    tw.Write(value);
  }

  public virtual void Write(string value, object arg0)
  {
    tw.Write(value, arg0);
  }

  public virtual void Write(string value, object arg0, object arg1)
  {
    tw.Write(value, arg0, arg1);
  }

  public virtual void Write(string value, object arg0, object arg1, object arg2)
  {
    tw.Write(value, arg0, arg1, arg2);
  }

  public virtual void Write(string value, object[] args)
  {
    tw.Write(value, args);
  }


  #region Implementation of IDisposable

  /// <summary>
  /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
  /// </summary>
  [ExcludeFromCodeCoverage]
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  [ExcludeFromCodeCoverage]
  protected virtual void Dispose(bool disposing)
  {
    if (!disposing) return;
    // free managed resources
    if (tw == null) return;
    tw.Dispose();
    tw = null;
  }

  #endregion
}