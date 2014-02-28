using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Realm.Library.Common
{
    public class TextReaderProxy : IDisposable
    {
        private TextReader _reader;

        public TextReaderProxy(TextReader tr)
        {
            _reader = tr;
        }

        [ExcludeFromCodeCoverage]
        ~TextReaderProxy()
        {
            Dispose(false);
        }

        public virtual int Read()
        {
            return _reader.Read();
        }

        public virtual string ReadLine()
        {
            return _reader.ReadLine();
        }

        public virtual string ReadToEnd()
        {
            return _reader.ReadToEnd();
        }

        public virtual bool EndOfStream
        {
            get { return _reader.Peek() == -1; }
        }

        public virtual char ReadNextLetter()
        {
            if (EndOfStream)
                throw new IOException("End of stream");
            return Convert.ToChar(Read());
        }

        public virtual int ReadNumber()
        {
            string word = ReadNextWord();

            int result;
            Int32.TryParse(word, out result);
            return result;
        }

        public virtual string ReadString(IEnumerable<char> terminatorChars = null)
        {
            string returnVal = string.Empty;
            List<char> terminatorCharacters = new List<char> { '\r', '\n' };
            if (terminatorChars != null)
                terminatorCharacters.AddRange(terminatorChars.ToList());

            do
            {
                char letter = ReadNextLetter();
                if (terminatorCharacters.Contains(letter))
                    break;
                returnVal += letter;

            } while (true);

            return returnVal;
        }

        public virtual string ReadToEndOfLine(bool includeCrlf = false)
        {
            string line = _reader.ReadLine();
            if (includeCrlf)
                _reader.ReadLine();
            return line;
        }

        public virtual string ReadNextWord()
        {
            string word = string.Empty;
            char c;
            do
            {
                c = Convert.ToChar(Read());
                if (c != ' ')
                    word += c;
            } while (c != ' ' && !EndOfStream);
            return word;
        }

        public List<string> ReadIntoList(char[] splitChars = null)
        {
            char[] charsToSplit = new[] { '\n' };

            return
                new List<string>(
                    ReadToEnd()
                        .Replace(Environment.NewLine, "\n")
                        .Replace("\n\r", "\n")
                        .Split(splitChars ?? charsToSplit, StringSplitOptions.RemoveEmptyEntries)
                        .ToList());
        }

        public List<TextSection> ReadSections(IEnumerable<string> headerChars = null,
                                              IEnumerable<string> commentChars = null,
                                              IEnumerable<string> footerChars = null, string endOfFile = "")
        {
            List<string> lines = ReadIntoList();
            List<TextSection> sections = new List<TextSection>();

            if (HasInvalidSectionParameters(headerChars, commentChars, footerChars, endOfFile))
                return sections;

            TextSection section = null;
            foreach (string line in lines
                .Where(line => commentChars == null || !commentChars.Any(line.StartsWith))
                .Where(line => String.IsNullOrWhiteSpace(endOfFile) || !line.StartsWith(endOfFile)))
            {
                if (headerChars.Any(c => line.StartsWith(c)))
                {
                    section = new TextSection {Header = line.Substring(1)};
                    sections.Add(section);
                    continue;
                }

                if (footerChars != null && footerChars.Any(c => line.StartsWith(c)))
                {
                    section.Footer = line.Substring(1);
                    continue;
                }

                section.Lines.Add(line);
            }
            return sections;
        }

        private static bool HasInvalidSectionParameters(IEnumerable<string> headerChars = null,
                                                        IEnumerable<string> commentChars = null,
                                                        IEnumerable<string> footerChars = null, string endOfFile = "")
        {
            return headerChars == null
                   || (commentChars != null && headerChars.Any(commentChars.Contains))
                   || (footerChars != null && headerChars.Any(footerChars.Contains))
                   || (!String.IsNullOrWhiteSpace(endOfFile) && headerChars.Contains(endOfFile));
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
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }
            }
        }

        #endregion
    }
}
