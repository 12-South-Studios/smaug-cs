using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Realm.Library.Common
{
    public class TextWriterProxy : IDisposable
    {
        private TextWriter _writer;

        public TextWriterProxy(TextWriter tw)
        {
            _writer = tw;
        }

        [ExcludeFromCodeCoverage]
        ~TextWriterProxy()
        {
            Dispose(false);
        }

        public virtual void Write(string value)
        {
            _writer.Write(value);
        }

        public virtual void Write(string value, object arg0)
        {
            _writer.Write(value, arg0);
        }

        public virtual void Write(string value, object arg0, object arg1)
        {
            _writer.Write(value, arg0, arg1);
        }

        public virtual void Write(string value, object arg0, object arg1, object arg2)
        {
            _writer.Write(value, arg0, arg1, arg2);
        }

        public virtual void Write(string value, object[] args)
        {
            _writer.Write(value, args);
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
            if (disposing)
            {
                // free managed resources
                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }
            }
        }

        #endregion
    }
}
