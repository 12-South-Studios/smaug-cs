﻿using System;
using System.ComponentModel;
using Library.Common.Attributes;

namespace Library.Common;

/// <summary>
/// WhitespaceDelimiter enumeration which is used to identify types of
/// delimiters that are used to separate words in strings.
/// </summary>
[Flags]
public enum DelimiterType
{
    [Enum("Whitespace", Value = 1)]
    [Description("\t\n\r ")]
    Whitespace,

    [Enum("Comma", Value = 2)]
    [Description(",")]
    Comma,

    [Enum("Period", Value = 4)]
    [Description(".")]
    Period,

    [Enum("Colon", Value = 8)]
    [Description(":")]
    Colon,

    [Enum("Equals", Value = 16)]
    [Description("=")]
    Equals,

    [Enum("Punctuation", Value = 32)]
    [Description(",:.=")]
    Punctuation,

    [Enum("Backslash", Value = 64)]
    [Description("/")]
    Backslash,

    [Enum("None", Value = 128)]
    None
}