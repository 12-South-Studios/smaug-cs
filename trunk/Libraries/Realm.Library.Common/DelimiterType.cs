using System;
using System.ComponentModel;

namespace Realm.Library.Common
{
    /// <summary>
    /// WhitespaceDelimiter enumeration which is used to identify types of
    /// delimiters that are used to separate words in strings.
    /// </summary>
    [Flags]
    public enum DelimiterType
    {
        [Enum("None", Value = 0)]
        None = 0,

        [Enum("Whitespace", Value = 1)]
        [Description("\t\n\r ")]
        Whitespace = 1,

        [Enum("Comma", Value = 2)]
        [Description(",")]
        Comma = 2,

        [Enum("Period", Value = 4)]
        [Description(".")]
        Period = 4,

        [Enum("Colon", Value = 8)]
        [Description(":")]
        Colon = 8,

        [Enum("Equals", Value = 16)]
        [Description("=")]
        Equals = 16,

        [Enum("Punctuation", Value = 32)]
        [Description(",:.=")]
        Punctuation = 32,

        [Enum("Backslash", Value = 64)]
        [Description("/")]
        Backslash = 64
    }
}