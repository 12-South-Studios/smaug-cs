using System.ComponentModel;

namespace Realm.Library.Common
{
    /// <summary>
    /// WhitespaceDelimiter enumeration which is used to identify types of
    /// delimiters that are used to separate words in strings.
    /// </summary>
    public enum DelimiterType
    {
#pragma warning disable 1591

        [Enum("Whitespace", 1)]
        [Description("\t\n\r ")]
        Whitespace,

        [Enum("Comma", 2)]
        [Description(",")]
        Comma,

        [Enum("Period", 4)]
        [Description(".")]
        Period,

        [Enum("Colon", 8)]
        [Description(":")]
        Colon,

        [Enum("Equals", 16)]
        [Description("=")]
        Equals,

        [Enum("Punctuation", 32)]
        [Description(",:.=")]
        Punctuation,

        [Enum("Backslash", 64)]
        [Description("/")]
        Backslash,

        [Enum("None", 128)]
        None

#pragma warning restore 1591
    }
}